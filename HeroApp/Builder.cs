using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HeroApp; 

internal static class Builder {
    public static readonly List< string > Abilities = new() {
        "Dexterity",
        "Intelligence",
        "Invisibility",
        "xRay Vision",
        "Flight",
        "Super Speed",
        "Super Strength",
        "Accelerated Healing",
        "Power Blast",
        "Telekinesis",
        "Telepathy",
        "Mind Control",
        "Time Travel",
        "Immortality",
        "Invulnerability",
        "Shape Shifting"
    };

    public static readonly List< string > Locations = new() {
        "New York",
        "Gotham",
        "Metropolis",
        "Star City",
        "Central City",
        "Coast City",
        "Atlantis",
        "Themyscira",
        "Asgard",
        "Wakanda",
        "Genosha",
        "Latveria",
        "Sokovia",
        "Kamar-Taj",
        "Hyrule",
        "Mushroom Kingdom",
        "Skyrim",
        "Pandora"
    };

    public static readonly List< string > PreferredTransportation = new() {
        "Paraglider",
        "Jetpack",
        "Batmobile",
        "Teleport",
        "Land Speeder",
        "TARDIS",
        "Starship"
    };

    public static readonly List< string > PreferredWeapon = new() {
        "Sword",
        "Bow",
        "Gun",
        "Lightsaber",
        "Hammer",
        "Shield",
        "Axe",
        "Dagger",
        "Staff",
        "Whip",
        "Fists",
        "Claws",
        "Mace",
        "Spear",
        "Chainsaw",
        "Flamethrower",
        "Grenade",
        "Laser",
        "Nunchucks",
    };
}