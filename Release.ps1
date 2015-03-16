cd Dev/Rpg1/bin

rmdir Release/Resources -Recurse
copy Debug/Resources Release -Recurse
cd Release/Resources

rm Effect/Absorb.efkproj
rm Effect/Shield.efkproj
rm Effect/Slash.efkproj
rm Music/swing_band.mp3
rm Music/è„å∑ÇÃåé.mp3
rm Music/üTëì.mp3
rm Font/achivement.csv
rm Font/number-list.txt
rm Font/shift-jis-list.txt

cd ..
./umwPacking.exe Resources Resources

cd ../../../../
MSBuild Dev/Rpg1.sln /p:Configuration=Release

pause