CD-Rには重くて入らなかったので導入から記述します

今回先にインストールしてほしいもの

Unity 5.6.3f1(これ以降ならばおそらく実行可能)
Kinect for windows ver2 (この時、faceもいれる)

1. まず、KinectとUnityちゃんの連携を行う場合

	BodySourceManager.cs、VectorExtensions.cs、KinectAvatar.csをすべてUNITY内に入れる。

	Unity内でUnitychanをインストール

	次にKinectAvater　を Hierarchy 内に作成し
	そこにKinectAvater.csをアタッチする。


	アタッチしたKinectAvaterの中にBodySourceManager,UNITYchanをいれる
	これでいけるはず



2. ロボットとの連携
	ロボットはパソコンをまずブルートゥース通信します
	その際にCOM番号を確認しておきます

	次にserialRをからのオブジェクトにアタッチします
	その際に中のプログラムのポート番号を書き換えておきます
	
	これで通信自体はできて
	いるので、行いたい処理を書き込んでおきます


3.顔の連携
	FaceToCamをUnitychanにアタッチします。
	その際にMTH,EYE,ELにそれぞれ同名の部位をアタッチします。
	これで大丈夫です。
	その際にBodySourceManegerもアタッチしておきます。

4 AR部分　Vuforia
	これはVuforiaで検索して出てくる手順通りにマーカーの登録、インストール、ARCameraとImageTargetをヒエラルキーにいれるなどを行います。
	その後、映したいUNITYChanをImageTargetの子供にすることでできます。
5.通信部分

	通信にはサーバー側と
	クライアント側を用意します
	通信のためにNetworkViewをアタッチします(Server側とClientどちらも)
	その後通信したい二つのアプリケーションそれぞれに用意してあるプログラムをそれぞれアタッチします

	・Client側(今回はこちらからサーバ側に骨格情報などを送る)
		ClientSideをアタッチして、項目内に必要なプログラムをアタッチします。
		こちら側にサーバー側のIPアドレスを入力しておきます。

	・Server側(今回はこちら側で受け取る)
		こちらもNetworkViewをアタッチしたものにServer Side NetWork Managerをアタッチする。
		特に書き換える項目はない。
		ただしこちら側ではKinectAvaterにアタッチが必要である。

以上になります。
もしわからないことがありましたら

1「Kinect v2 UNITYちゃん」　で検索
2「BlueTooth Arduino」で検索
3は検索しても直接的な記事が出てくるかわかりません。
4これも直接的な記事は出てきません。RPCを二つに用意し、同名関数で呼び出すことでどちらも行っていると考えてください。

