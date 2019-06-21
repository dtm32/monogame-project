/* http://www.convertcsv.com/csv-to-json.htm
   Create Custom Output via Template

   TOP: {lb}{br}
   MID: 
"{f2}": {lb}
  "{h1}":"{f1}",
  "{h2}":"{f2}",
  "{h3}":{(f3)==""?"null":f3},
  "{h4}":{(f4)==""?"null":f4},
  "{h5}":{(f5)==""?"null":f5},
  "{h6}":{(f6)==""?"null":f6},
  "{h7}":{(f7)==""?"null":f7},
  "{h8}":{(f8)==""?"null":f8},
  "{h9}":{(f9)==""?"null":f9}
{rb}
   BOT: {br}{rb}
*/
const UNITS = {
  "Looter": {
    "Type":"Infantry",
    "Name":"Looter",
    "HP":56,
    "Spd":27,
    "Str":36,
    "Fcs":6,
    "Amr":21,
    "Res":43,
    "Total":189
  },
  "Heavy Looter": {
    "Type":"Infantry",
    "Name":"Heavy Looter",
    "HP":64,
    "Spd":25,
    "Str":39,
    "Fcs":3,
    "Amr":49,
    "Res":19,
    "Total":199
  },
  "Deviant": {
    "Type":"Mage",
    "Name":"Deviant",
    "HP":48,
    "Spd":30,
    "Str":11,
    "Fcs":44,
    "Amr":20,
    "Res":28,
    "Total":181
  },
  "Possesed Deviant": {
    "Type":"Mage",
    "Name":"Possesed Deviant",
    "HP":49,
    "Spd":44,
    "Str":0,
    "Fcs":54,
    "Amr":23,
    "Res":33,
    "Total":203
  },
  "Possesed Traveler": {
    "Type":"Mage",
    "Name":"Possesed Traveler",
    "HP":34,
    "Spd":60,
    "Str":0,
    "Fcs":37,
    "Amr":20,
    "Res":11,
    "Total":162
  },
  "Rouge Traveler": {
    "Type":"Infantry",
    "Name":"Rouge Traveler",
    "HP":43,
    "Spd":58,
    "Str":36,
    "Fcs":13,
    "Amr":23,
    "Res":25,
    "Total":198
  },
  "Light Bowman": {
    "Type":"Infantry",
    "Name":"Light Bowman",
    "HP":45,
    "Spd":45,
    "Str":45,
    "Fcs":0,
    "Amr":11,
    "Res":19,
    "Total":165
  },
  "Hunter": {
    "Type":"Infantry",
    "Name":"Hunter",
    "HP":36,
    "Spd":49,
    "Str":39,
    "Fcs":0,
    "Amr":16,
    "Res":10,
    "Total":150
  },
  "Possesed Hunter": {
    "Type":"Mage",
    "Name":"Possesed Hunter",
    "HP":29,
    "Spd":49,
    "Str":0,
    "Fcs":43,
    "Amr":16,
    "Res":16,
    "Total":153
  },
  "Summoner": {
    "Type":"Mage",
    "Name":"Summoner",
    "HP":50,
    "Spd":49,
    "Str":0,
    "Fcs":63,
    "Amr":7,
    "Res":33,
    "Total":202
  },
  "Kensick Legionnaire": {
    "Type":"Infantry",
    "Name":"Kensick Legionnaire",
    "HP":77,
    "Spd":35,
    "Str":45,
    "Fcs":3,
    "Amr":50,
    "Res":35,
    "Total":245
  },
  "Kensick Knight": {
    "Type":"Knight",
    "Name":"Kensick Knight",
    "HP":85,
    "Spd":22,
    "Str":55,
    "Fcs":0,
    "Amr":69,
    "Res":14,
    "Total":245
  },
  "Kensick Royal Legionaire": {
    "Type":"Infantry",
    "Name":"Kensick Royal Legionaire",
    "HP":80,
    "Spd":39,
    "Str":51,
    "Fcs":2,
    "Amr":51,
    "Res":42,
    "Total":265
  },
  "Kensick Royal Knight": {
    "Type":"Knight",
    "Name":"Kensick Royal Knight",
    "HP":96,
    "Spd":17,
    "Str":64,
    "Fcs":0,
    "Amr":75,
    "Res":8,
    "Total":260
  },
  "Kensick Royal Guard": {
    "Type":"Knight",
    "Name":"Kensick Royal Guard",
    "HP":72,
    "Spd":30,
    "Str":51,
    "Fcs":32,
    "Amr":48,
    "Res":42,
    "Total":275
  },
  "Kensick Royal Conjurer": {
    "Type":"Mage",
    "Name":"Kensick Royal Conjurer",
    "HP":70,
    "Spd":42,
    "Str":0,
    "Fcs":50,
    "Amr":51,
    "Res":42,
    "Total":255
  },
  "Dark Knight": {
    "Type":"Knight",
    "Name":"Dark Knight",
    "HP":84,
    "Spd":15,
    "Str":65,
    "Fcs":5,
    "Amr":45,
    "Res":40,
    "Total":254
  }
}


// unit object
function Unit(pStats, pWeapon)
{
  let vHealth, vSpeed, vStrength, vFocus, vArmor, vResistance;
  let vWeapon;
  // power: 0,
  // effect: [], enum of all possible effects
  // description: ''

  this.vHealth     = Integer.parseInt(pStats.HP);
  this.vSpeed      = Integer.parseInt(pStats.Spd);
  this.vStrength   = Integer.parseInt(pStats.Str);
  this.vFocus      = Integer.parseInt(pStats.Fcs);
  this.vArmor      = Integer.parseInt(pStats.Amr);
  this.vResistance = Integer.parseInt(pStats.Res);
  this.vWeapon     = pWeapon;

  let calculateDamage = function(pDefender)
  {

  }

  let getHealth = function()
  {
    return this.vHealth;
  }

  let getSpeed = function()
  {
    return this.vSpeed;
  }

  let getStrength = function()
  {
    return this.vStrength;
  }

  let getFocus = function()
  {
    return this.vFocus;
  }

  let getArmor = function()
  {
    return this.vArmor;
  }

  let getResistance = function()
  {
    return this.vResistance;
  }
}