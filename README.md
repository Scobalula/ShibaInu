![Shiba](https://i.imgur.com/HB2PmiZ.png)
# ShibaInu

ShibaInu is the tool that will simplifier your weapon porting. It allows you to convert legacy weapon files and CSV weapon tables (such as Marvel's **Raw** tables with stats layed out in a CSV) to a GDT with ease. Shiba uses templates which you can add to, modify, etc. to make your own to suit your needs.

# Using Shiba

To start, download the latest [Release](https://github.com/Scobalula/ShibaInu/releases). Once downloaded, you can start Shiba and load your weapon files and/or CSVs to convert weapons.

To open a weapon file or CSV, you can either drag and drop them onto the weapons list, press Load Weapon/s, or press CTRL+O.

To save selected weapons, press Save Selected or press CTRL+S.

To save all weapons, press Save All or press CTRL+SHIFT+S.

To change the template for a weapon, select the weapon, then select the template you'd like to use for that weapon, the selected weapon will retain this setting.

To clear loaded weapons, press CTRL+SHIFT+X.

# Editing Templates

On startup, Shiba will load all GDT files in the WeaponTemplates folder and look for the asset called "template". The name of the GDT file does not reflect the asset type output of the weapon, as Shiba will use the asset within the GDT. With this in mind, you can copy GDTs to make templates for use with different weapons. 

To create new templates from APE, you can copy your asset to its own GDT, and copy that GDT to the WeaponTemplates folder.

To exclude settings from being copied from the resulting weapon, you can add `__EXCLUDE__` without spaces, to the setting's property, which will result in Shiba just using the value in the template instead of the weapon. An example of this being used:

```
		"raiseSound" "fly_generic_raise_npc__EXCLUDE__"
		"raiseSoundPlayer" "fly_generic_raise_plr"
		"raiseTime" "0.9"
		"rechamberAnim" ""
		"rechamberBoltTime" "0"
		"rechamberSound" ""
		"rechamberSoundPlayer" ""
		"rechamberTime" "0.1"
		"rechamberWhileAds" "1"
		"reloadAddTime" "1.53"
		"reloadAmmoAdd" "0"
		"reloadAnim" "vm_ap9_reload"
		"reloadEmptyAddTime" "0"
		"reloadEmptyAnim" "vm_ap9_reload_empty"
		"reloadEmptySound" ""
		"reloadEmptySoundPlayer" ""
		"reloadEmptyTime" "3.16__EXCLUDE__"
		"reloadEndAnim" ""
		"reloadEndSound" "__EXCLUDE__"
		"reloadEndSoundPlayer" ""
		"reloadEndTime" "0"
```
## License / Disclaimers

ShibaInue is licensed under the GPL license and it and its source code is free to use and modify under the . ShibaInu comes with NO warranty, any damages caused are solely the responsibility of the user. See the LICENSE file for more information.

## Download

The latest version can be found on the [Releases Page](https://github.com/Scobalula/ShibaInu/releases).

If you're looking for weapon files and tables that can be loaded into Shiba, check out Marvel's stats post (for CSVs, you must use **Raw** tables with WEAPONFILE as the first column):

[http://denkirson.proboards.com/thread/4943/call-duty-weapon-attachment-stats](http://denkirson.proboards.com/thread/4943/call-duty-weapon-attachment-stats)

## Credits

* raptroes - Requesting it

## Support Me

If you use Shiba in any of your projects, it would be appreciated if you provide credit for its use, a lot of time and work went into developing it and a simple credit isn't too much to ask for.

If you'd like to support me even more, consider donating, I develop a lot of apps including Shiba and majority are available free of charge with source code included:

[![Donate](https://img.shields.io/badge/Donate-PayPal-yellowgreen.svg)](https://www.paypal.me/scobalula)
