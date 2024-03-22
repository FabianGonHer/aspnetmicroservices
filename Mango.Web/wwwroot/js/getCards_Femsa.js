
var gravtyUrl = "http://api.uso20.gravty.io/v1/entity-data/members/member_cards/?filter(member_id)=5PAU8SN";
var sponsor = "";
var gravty_vertical_entities = [];
var cardsComplete = [];
var cardsCompleteChange = [];

function getVerticalEntities() {
    fetch(gravtyUrl, {
        method: 'GET',
        headers: {
            'Accept': '*/*',
            'Authorization': 'JWT ',
            'X-Api-Key': 'iIJ3pbwkdg2kyoobBOt852pMM9HhOPKEaD84vtMZ'
        }
    })
        .then(response => response.json())
        .then(entities => {
            cardsComplete = entities;
            cardsCompleteChange = entities;
            gravty_vertical_entities = entities.data
        });
}

//var gravty_vertical_entities = JSON.parse(context.getVariable("custom.gravty_vertical_entities"));
//var cardsComplete = JSON.parse(context.getVariable("vertical_entity.content"));
//var cardsCompleteChange = JSON.parse(context.getVariable("vertical_entity.content"));

function searchSponsor(medium_code) {
    var sponsors = [
        {
            name: "oxxo",
            prefixes: ["914", "925"],
            sponsor_label: "premia"
        },
        {
            name: "spin",
            prefixes: ["917", "924"],
            sponsor_label: "spin"
        }
    ];
    var sponsor_name = "";
    sponsors.forEach((sponsor) => {
        sponsor.prefixes.forEach((prefix) => {
            if (medium_code.substring(0, 3) == prefix) {
                sponsor_name = sponsor.sponsor_label;
            }
        });
    });
    return sponsor_name;
}

function cardsProcess(cards) {
    var valid_cards = [];
    cards.forEach((cards) => {
        if (cards.status === "ACTIVE" && cards.valid === true) {
            // var sponsor = searchSponsor(cards.medium_code);
            var sponsor = (cards.repeat !== undefined) ? "premia" : searchSponsor(cards.medium_code);
            cards.medium_type = cards.medium_type === 'virtual' ? 'digital' : 'physical';
            cards.allow_accrual = cards.allow_accrual === 'YES' ? true : false;
            cards.allow_redemption = (cards.allow_redemption === 'YES' || cards.allow_redemption === null) ? true : false;
            cards.temporarily_blocked = (cards.temporary_block === 'NO' || cards.temporary_block === null) ? false : true;
            cards.sponsor_name = sponsor;
            valid_cards.push(cards);
        }
    });
    return valid_cards;
}

var cardsAdd = [];
cardsCompleteChange.data.forEach((cards_complete) => {
    if (cards_complete.status === "ACTIVE" && cards_complete.valid === true && cards_complete.medium_code.substring(0, 3) === "924") {
        cards_complete.repeat = true;
        cardsAdd.push(cards_complete);
    }
});

Array.prototype.push.apply(cardsComplete.data, cardsAdd);

var filtered = cardsProcess(cardsComplete.data.reverse());

var group = filtered.reduce((reg, curr) => {
    var found = reg.find((x) => x.sponsor === curr.sponsor_name);
    if (found) {
        var nuevofound = {
            type: curr.medium_type,
            medium_code: curr.medium_code,
            redeem: curr.allow_redemption,
            temporarily_blocked: curr.temporarily_blocked,
            status: "assigned"
        };
        found.mediums.push(nuevofound);
    }
    else {
        var nuevo = {
            type: curr.medium_type,
            medium_code: curr.medium_code,
            redeem: curr.allow_redemption,
            temporarily_blocked: curr.temporarily_blocked,
            status: "assigned"
        };
        reg.push({
            sponsor: curr.sponsor_name,
            mediums: [nuevo]
        });
    }
    return reg;
}, []);

group.forEach((reg) => {
    var count = 0;
    reg.mediums.forEach((card) => {
        if (card.type !== "digital" && card.redeem !== true) {
            count++;
        }
    });
    reg.mediums.forEach((card, index) => {
        if (card.type !== "digital" && card.redeem !== true) {
            if (count == 1) {
                // if(index===0){
                card.status = "pending";
                // }
            } else if (count > 1) {
                if (index === 0) {
                    card.status = "new_to_replace";
                } else if (index == 1) {
                    card.status = "pending_to_replace";
                }
            }
        }
    });
    reg.mediums.reverse();
});

var data = {
    data: group
};
