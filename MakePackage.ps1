rmdir Release -Recurse
mkdir Release

cd Dev/Rpg1/bin/Release
.\ILMerge.exe Rpg1.Ace.exe NacHelpers.dll Rpg1.Model.dll /targetplatform:v4 /out:Merged.exe

function toPack($file) { copy $file ../../../../Release }

toPack Merged.exe
toPack Resources.pack
toPack ace_core.dll
toPack ace_cs.dll
toPack CsvHelper.dll
toPack System.Interactive.dll

cd ../../../../Release
mv Merged.exe ñ`åØé“ÇÕêXÇ…ã≠Ç¢.exe

cd ../
cp license.txt Release
cp readme.txt Release

pause