CD-R�ɂ͏d���ē���Ȃ������̂œ�������L�q���܂�

�����ɃC���X�g�[�����Ăق�������

Unity 5.6.3f1(����ȍ~�Ȃ�΂����炭���s�\)
Kinect for windows ver2 (���̎��Aface�������)

1. �܂��AKinect��Unity�����̘A�g���s���ꍇ

	BodySourceManager.cs�AVectorExtensions.cs�AKinectAvatar.cs�����ׂ�UNITY���ɓ����B

	Unity����Unitychan���C���X�g�[��

	����KinectAvater�@�� Hierarchy ���ɍ쐬��
	������KinectAvater.cs���A�^�b�`����B


	�A�^�b�`����KinectAvater�̒���BodySourceManager,UNITYchan�������
	����ł�����͂�



2. ���{�b�g�Ƃ̘A�g
	���{�b�g�̓p�\�R�����܂��u���[�g�D�[�X�ʐM���܂�
	���̍ۂ�COM�ԍ����m�F���Ă����܂�

	����serialR������̃I�u�W�F�N�g�ɃA�^�b�`���܂�
	���̍ۂɒ��̃v���O�����̃|�[�g�ԍ������������Ă����܂�
	
	����ŒʐM���̂͂ł���
	����̂ŁA�s��������������������ł����܂�


3.��̘A�g
	FaceToCam��Unitychan�ɃA�^�b�`���܂��B
	���̍ۂ�MTH,EYE,EL�ɂ��ꂼ�ꓯ���̕��ʂ��A�^�b�`���܂��B
	����ő��v�ł��B
	���̍ۂ�BodySourceManeger���A�^�b�`���Ă����܂��B

4 AR�����@Vuforia
	�����Vuforia�Ō������ďo�Ă���菇�ʂ�Ƀ}�[�J�[�̓o�^�A�C���X�g�[���AARCamera��ImageTarget���q�G�����L�[�ɂ����Ȃǂ��s���܂��B
	���̌�A�f������UNITYChan��ImageTarget�̎q���ɂ��邱�Ƃłł��܂��B
5.�ʐM����

	�ʐM�ɂ̓T�[�o�[����
	�N���C�A���g����p�ӂ��܂�
	�ʐM�̂��߂�NetworkView���A�^�b�`���܂�(Server����Client�ǂ����)
	���̌�ʐM��������̃A�v���P�[�V�������ꂼ��ɗp�ӂ��Ă���v���O���������ꂼ��A�^�b�`���܂�

	�EClient��(����͂����炩��T�[�o���ɍ��i���Ȃǂ𑗂�)
		ClientSide���A�^�b�`���āA���ړ��ɕK�v�ȃv���O�������A�^�b�`���܂��B
		�����瑤�ɃT�[�o�[����IP�A�h���X����͂��Ă����܂��B

	�EServer��(����͂����瑤�Ŏ󂯎��)
		�������NetworkView���A�^�b�`�������̂�Server Side NetWork Manager���A�^�b�`����B
		���ɏ��������鍀�ڂ͂Ȃ��B
		�����������瑤�ł�KinectAvater�ɃA�^�b�`���K�v�ł���B

�ȏ�ɂȂ�܂��B
�����킩��Ȃ����Ƃ�����܂�����

1�uKinect v2 UNITY�����v�@�Ō���
2�uBlueTooth Arduino�v�Ō���
3�͌������Ă����ړI�ȋL�����o�Ă��邩�킩��܂���B
4��������ړI�ȋL���͏o�Ă��܂���BRPC���ɗp�ӂ��A�����֐��ŌĂяo�����Ƃłǂ�����s���Ă���ƍl���Ă��������B

