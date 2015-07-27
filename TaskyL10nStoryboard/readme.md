Tasky (localized storyboard)
============================

You can localize all the strings used in a storyboard by creating a `.strings`
file named the same as the storyboard (placed in relevant `.lproj` directory)
and creating strings for each control/property, eg:

**es.lproj/MainStoryboard.strings**

```
"SXg-TT-IwM.placeholder" = "nombre de la tarea";
"Pqa-aa-ury.placeholder"= "﻿otra información de tarea";
"zwR-D9-hM1.text" = "Detalles de la tarea";
"bAM-2j-Rzw.text" = "Notas";
"NF3-h8-xmR.text" = "Completo";
"MWt-Ya-pMf.normalTitle" = "Guardar";
"IGr-pR-05L.normalTitle" = "Eliminar";
```

where the first part of each key is the control's Localization ID in the storyboard,
and the second part is the string property to set (eg a UITextInput's `placeholder`
property, or a UILabel's `text`).

Don't forget these entries in the `Info.plist` (for the languages you wish to support):

```
<key>CFBundleLocalizations</key>
<array>
  <string>de</string>
  <string>es</string>
  <string>ja</string>
</array>
<key>CFBundleDevelopmentRegion</key>
<string>en</string>
```

**WARNING:** This sample does *not* use Size Classes, but rather is specifically
targeting iPhone in the storyboard.

If you attempt to localize a Storyboard that uses Size Classes you may
run into [this issue (on StackOverflow)](http://stackoverflow.com/questions/24989208/xcode-6-does-not-localize-interface-builder).
The storyboard elements fail

The easy/hack fix is to duplicate your storyboard strings file into
`MainStoryboard~iphone.strings` and `MainStoryboard~ipad.strings`

![](Screenshots/es-sml.png)
