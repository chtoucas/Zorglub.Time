// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

namespace Zorglub.Time;

// Dans les calendriers julien et grégorien, le jour de l'an est
// fixé au 1er janvier (circoncision de J.-C.), cependant l'année pouvait
// dans les faits commencer un tout autre jour : le 1er mars, le 25 décembre
// (naissance de J.-C.), Pâques (résurrection de J.-C.) --- complique
// sacrément les choses puisque la date n'est pas fixe et les années ont une
// durée variable ---, le 25 mars (annonciation de J.-C.), le 1er vendémiaire
// (calendrier révolutionnaire), etc.
// Chaque changement de style (type de datation du début de l'année) se fait
// progressivement, mais on a approximativement :
// - 25 décembre, depuis Charlemagne (?) jusqu'au XIe siècle environ
// - Pâques, aux XIIe et XIIIe siècles environ
// - 25 décembre, du XIVe au XVIe siècles environ
// - 1er janvier après 1563/1567.
// L'année commence le 1er janvier depuis l'édit de Roussillon de 1564,
// il ne s'agit en fait que de la confirmation de l'édit de Paris qui, lui,
// date de 1563. Dans les faits, cet édit entra pleinement en vigueur plus
// tardivement, en 1567.
// Lorsqu'on consulte des actes anciens, on doit donc faire attention
// à un éventuel décalage : p.ex. le 1er janvier 1565 (ancien style,
// abrégé a.s.) correspond en fait au 1er janvier 1566 (nouveau style,
// abrégé n.s.) --- la période problématique va de janvier à Pâques.
//
// Références :
// - http://www.francegenweb.org/wiki/index.php?title=Calendrier
// - https://fr.wikipedia.org/wiki/Jour_de_l%27an
// - https://fr.wikipedia.org/wiki/%C3%89dit_de_Roussillon
// - https://fr.wikipedia.org/wiki/Ann%C3%A9e_z%C3%A9ro
public enum NewYearStyle
{
    // 1er janvier : circoncision de J.-C.
    Default = 0,

    // 25 décembre : naissance de J.-C.
    Nativity,

    // Date mobile : jour de Pâques.
    Easter,

    // 25 mars
    Annunciation,

    // Date mobile : équinoxe à Paris.
    Republican,

    // 1er mars : mois du dieu de la guerre.
    // Au passage, ceci explique les noms des mois de septembre (7),
    // octobre (8), novembre (9) et décembre (10).
    Martius,
}
