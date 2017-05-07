# 冒険者は森に強い
３すくみのスキルを駆使して敵を連続で倒し続けるゲームです。

ゲームに使用されている画像、音声、エフェクトファイルなどの素材をコピーして別の用途に使用しないでください。

このソースコードは勉強用です。個人的な勉強以外の二次使用はしないでください。このライセンスは変更する可能性があります。

## 設計解説

少し黒魔術チックですが、コルーチンを用いてゲームの流れを記述しています。
[コルーチンを使ったゲームフロー管理](https://github.com/NumAniCloud/StrongAdventurer/blob/master/Description/Coroutine.md) もご参照ください。

## ファイル構造解説

### Resources.pack

`Dev/Rpg1/bin/Debug/Resources.pack`ファイルには画像などのリソースが入っています。権利などに配慮して、アーカイブされたリソースのみを同梱していますのでご了承ください。

### Dataフォルダ

`Dev/Rpg1/bin/Debug/Data`フォルダにはゲームのデータベースが入っています。このcsvファイルと同一のものがResources.packに入っており、そこからロードしています。

### ace_core.dll, ace_cs.dll

`Dev/Rpg1/bin/Debug/ace_core.dll`ファイルなどはナムアニクラウドが開発に参加しているゲームエンジン「ACE」のライブラリです。起動に必要です。[ACEについてはこちら](https://github.com/ac-engine/amusement-creators-engine/blob/master/Document/Index.md)

### Release.ps1

Releaseビルドをするスクリプトですが、リソースをアーカイブしか提供していない関係上GitHub版では動作しませんし、使う必要はありません。

### MakePackage.ps1

ゲームのパッケージを作成するスクリプトです。実行する前にReleaseビルドをしておく必要があります。また、ILMerge.exeがReleaseフォルダ内にあることを想定しているので、前もってそれを置いておかないと動きません。
