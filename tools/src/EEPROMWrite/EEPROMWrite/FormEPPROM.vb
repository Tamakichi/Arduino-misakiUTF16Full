'
' arduino Mega2560�pEEPROM�f�[�^�������݃c�[��
' �ŏI�C���� 2014/02/28 v2.0
' ���ӎ���: �varduino���ɐ�p�\�t�g�̃C���X�g�[�� 
'

Imports System.Threading
Public Class FormEPPROM

    Dim datafpath As String     '�f�[�^�t�@�C���p�X
    Dim addr As UInt32          '�f�[�^�������ݐ擪�A�h���X
    Dim flgActive As Boolean    '�]�����t���O(0:���]�� 1:�]����)
    Dim mThread As Thread       '�f�[�^�]���p�X���b�h
    Dim jobsts As Boolean       '�X���b�h�ғ�����

    'Delegate Sub AddDataDelegate(ByVal str As String)

    '�f�[�^�t�@�C���̎w��
    Private Sub BtnFopen_Click(sender As Object, e As EventArgs) Handles BtnFopen.Click
        OpenFileDialogData.ShowDialog()
        datafpath = OpenFileDialogData.FileName
        TB_Fname.Text = datafpath
    End Sub

    '���O���b�Z�[�W�̏�������
    Private Sub addLog(ByVal str As String)
        Tb_message.Text = Tb_message.Text + str + vbCrLf
    End Sub

    '�R���g���[������̐���
    Private Sub ctrlSetting()
        If flgActive = True Then
            Cmb_comport.Enabled = False
            TB_Fname.Enabled = False
            BtnFopen.Enabled = False
            tb_addr.Enabled = False
            BtnWriteData.Enabled = False
            btn_stop.Enabled = True
        Else
            Cmb_comport.Enabled = True
            TB_Fname.Enabled = True
            BtnFopen.Enabled = True
            tb_addr.Enabled = True
            BtnWriteData.Enabled = True
            btn_stop.Enabled = False
        End If
    End Sub


    '�f�[�^�������ݏ���
    '�@�o�b�N�O�����h�p�X���b�h����Ăяo�����
    Private Sub Upload()
        Dim data As Array
        Dim rcv As Integer
        Dim sdt(6) As Byte
        Dim wdt(1) As Byte
        Dim cnt As UInt32
        Dim flgend As Boolean
        Dim cksum As Integer

        flgend = False

        cksum = 0

        '�]������ԂɈڍs
        flgActive = True
        Me.BeginInvoke( _
        Sub()
            addLog("�o�b�N�O���E���h�ŒʐM�������J�n���܂���.")
            ctrlSetting()
        End Sub)


        '** �����ݗp�f�[�^�̃��[�h **
        Try
            data = My.Computer.FileSystem.ReadAllBytes(datafpath)
        Catch ex As Exception
            Me.BeginInvoke( _
                Sub()
                    MessageBox.Show(ex.Message, Me.Text, MessageBoxButtons.OK, MessageBoxIcon.Error)
                    addLog("�f�[�^�t�@�C����ǂݍ��݂Ɏ��s���܂���.�������I�����܂�.")
                End Sub)
            flgActive = False
            Exit Sub
        End Try
        Me.BeginInvoke( _
            Sub()
                addLog("�f�[�^�t�@�C����ǂݍ��݂܂���. �T�C�Y:" & data.Length)
            End Sub)

        '** �������݊J�n�A�h���X���M,�f�[�^�T�C�Y 6�o�C�g���M
        sdt(0) = (addr >> 16) And &HFF
        sdt(1) = (addr >> 8) And &HFF
        sdt(2) = addr And &HFF
        sdt(3) = (data.Length >> 16) And &HFF
        sdt(4) = (data.Length >> 8) And &HFF
        sdt(5) = data.Length And &HFF

        Try
            '�R�}���h���M
            SerialPort1.Write("d")
            SerialPort1.Write(sdt, 0, 6)
        Catch ex As Exception
            Me.BeginInvoke( _
                Sub()
                    MessageBox.Show(ex.Message, Me.Text, MessageBoxButtons.OK, MessageBoxIcon.Error)
                    addLog("�J�n�A�h���X���M,�f�[�^�T�C�Y�̑��M�Ɏ��s���܂���.")
                End Sub)
            flgActive = False
            Exit Sub
        End Try

        '������M
        Try
            rcv = SerialPort1.ReadByte()
        Catch ex As Exception
            Me.BeginInvoke( _
                Sub()
                    MessageBox.Show(ex.Message, Me.Text, MessageBoxButtons.OK, MessageBoxIcon.Error)
                    addLog("�J�n�A�h���X���M,�f�[�^�T�C�Y�̑��M�̉�����M�Ɏ��s���܂���.")
                End Sub)
            flgActive = False
            Exit Sub
        End Try
        If rcv <> Asc("a") Then
            Me.BeginInvoke( _
                Sub()
                    addLog("�J�n�A�h���X���M,�f�[�^�T�C�Y�̑��M�̉�����M(NAK)�Ɏ��s���܂���.")
                End Sub)
            flgActive = False
            Exit Sub
        End If

        cnt = 0
        Me.BeginInvoke( _
            Sub()
                addLog("�J�n�A�h���X���M,�f�[�^�T�C�Y�̑��M����.")
                addLog("�f�[�^���M�J�n")
                Lbl_status.Text = "��: �ʐM�� [" & cnt & "/" & data.Length & "]"
                Lbl_status.Update()
            End Sub)

        '** �ȍ~�f�[�^���M����
        ' �P�o�C�g���M�̓s�x�A��������M���Ă���
        For Each d As Byte In data

            '1�o�C�g���M
            wdt(0) = d
            SerialPort1.Write(wdt, 0, 1)

            cksum = cksum + d
            cksum = cksum And &HFFFF

            '������M
            rcv = SerialPort1.ReadByte()

            '��������
            If rcv = Asc("o") Then
                '�����ʒm��M
                cnt = cnt + 1
                Me.BeginInvoke( _
                Sub()
                    addLog("�f�[�^���M����.")
                    Lbl_status.Text = "��: �]������ [" & cnt & "/" & data.Length & "]"
                    Lbl_status.Update()
                End Sub)
                flgend = True
                Exit For
            End If
            If rcv <> Asc(".") Then
                '�����ݎ��s
                Me.BeginInvoke( _
                Sub()
                    addLog("EEPROM�̏������݂Ɏ��s.")
                End Sub)
                Exit For
            End If

            '�i���\��
            cnt = cnt + 1
            If cnt Mod 100 = 0 Then
                Me.BeginInvoke( _
                Sub()
                    Lbl_status.Text = "��: �ʐM�� [" & cnt & "/" & data.Length & "]"
                    Lbl_status.Update()
                End Sub)
            End If

            If jobsts = False Then
                Me.BeginInvoke( _
                Sub()
                    addLog("�ʐM�𒆎~���܂���.")
                    Lbl_status.Text = "��: ���~ [" & cnt & "/" & data.Length & "]"
                    Lbl_status.Update()
                End Sub)
                Exit For
            End If
        Next

        '�ʐM�I��
        If flgend = True Then
            '�f�[�^���M����I��
            Dim sum_l As Integer
            Dim sum_h As Integer
            Dim rcv_sum As Integer

            '�`�F�b�N�T���l�擾�v��
            SerialPort1.Write("s")
            sum_l = SerialPort1.ReadByte()
            sum_h = SerialPort1.ReadByte()
            rcv_sum = (sum_h << 8) + sum_l

            '�o���̃`�F�b�N�T���̔�r
            Dim msg As String
            If cksum <> rcv_sum Then
                msg = "�`�F�b�N�T���l����v���܂���. ��:" & Hex(cksum) & " ��:" & Hex(rcv_sum)
            Else
                msg = "�`�F�b�N�T���l����v���܂���. ��:" & Hex(cksum) & " ��:" & Hex(rcv_sum)
            End If
            Call SerialPort1.Close()
            Me.BeginInvoke( _
            Sub()
                addLog(msg)
            End Sub)
        End If

        If SerialPort1.IsOpen = True Then
            Call SerialPort1.Close()
            Me.BeginInvoke( _
            Sub()
                addLog("�V���A���ڑ���ؒf���܂���.")
                ctrlSetting()
            End Sub)
        End If

        '��ʐM��ԂɈڍs
        flgActive = False
        Me.BeginInvoke( _
        Sub()
            addLog("�o�b�N�O���E���h�������I�����܂���.")
            ctrlSetting()
        End Sub)

    End Sub

    '�f�[�^�������݃{�^��
    Private Sub BtnWriteData_Click(sender As Object, e As EventArgs) Handles BtnWriteData.Click

        '���O�̃N���A
        Tb_message.Text = ""

        '�V���A���ڑ�
        Try
            SerialPort1.PortName = Cmb_comport.Text
            If SerialPort1.IsOpen = True Then
                MessageBox.Show("���ł�" & SerialPort1.PortName & "�͐ڑ�����Ă��܂��B", "�G���[", MessageBoxButtons.OK, MessageBoxIcon.Error)
            End If
            Call SerialPort1.Open()
        Catch ex As Exception
            MessageBox.Show(ex.Message, "�G���[", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
        addLog("�V���A���ڑ����܂���.")

        '�f�[�^�t�@�C���̃`�F�b�N
        If datafpath = "" Then
            MessageBox.Show("�f�[�^�t�@�C�����w�肳��Ă��܂���", "�G���[", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Exit Sub
        End If

        '�����݊J�n�A�h���X�̎擾
        addr = Convert.ToInt32(tb_addr.Text, 16)

        '�����ݗp�X���b�g�̎��s
        flgActive = True
        jobsts = True
        ctrlSetting()
        mThread.Start() '�X���b�h�̊J�n
    End Sub

    '�f�[�^�t�@�C���̓��͕ύX��ϐ��ɔ��f������
    Private Sub TB_Fname_TextChanged(sender As Object, e As EventArgs) Handles TB_Fname.TextChanged
        datafpath = TB_Fname.Text
    End Sub

    '�����݊J�n�A�h���X���̓e�L�X�g�{�b�N�X�̓��͐���w��
    Private Sub tb_addr_KeyPress(sender As Object, e As KeyPressEventArgs) Handles tb_addr.KeyPress
        If Asc(e.KeyChar) > 20 Then
            If InStr("0123456789ABCDEFabcdef", e.KeyChar) = 0 Then
                e.Handled = True
            End If
        End If
    End Sub

    '�f�[�^�]���p�̃X���b�h�{��
    Private Sub job_thread()
        Upload()
    End Sub

    '[�I��]�{�^��������������
    Private Sub Btn_end_Click(sender As Object, e As EventArgs) Handles Btn_end.Click
        Me.Close()
    End Sub

    '�v���O�����I�����̏���([�I��]�{�^���A[�~]�{�^����)
    Private Sub FormEPPROM_FormClosing(sender As Object, e As FormClosingEventArgs) Handles Me.FormClosing
        If flgActive = True Then
            If MessageBox.Show("�f�[�^�ʐM���ł����I�����܂����H", Me.Text, MessageBoxButtons.OKCancel, MessageBoxIcon.Question) = Windows.Forms.DialogResult.Cancel Then
                e.Cancel = True
            End If
        End If
    End Sub

    'Form���[�h���̏���������
    Private Sub FormEPPROM_Load(sender As Object, e As EventArgs) Handles Me.Load

        '�V���A���|�[�g�̈ꗗ���擾���R���{�{�b�N�X�ɕ\������
        Dim ports() As String = IO.Ports.SerialPort.GetPortNames()
        For Each str As String In ports
            Cmb_comport.Items.Add(str)
            If ports.Length > 0 Then
                Cmb_comport.SelectedIndex = 0
            End If
        Next

        '����{�^���̑��쐧��ݒ�
        flgActive = False
        ctrlSetting()

        '�f�[�^�]���p�̃X���b�h�̍쐬
        mThread = New Thread(AddressOf job_thread) '�o�b�N�O���E���h�����̐ݒ�
        mThread.IsBackground = True '�o�b�N�O���E���h�E�X���b�h�ɂ���

    End Sub

    '[���~]�{�^���ɂ�钆�~����
    Private Sub btn_stop_Click(sender As Object, e As EventArgs) Handles btn_stop.Click
        If MessageBox.Show("�f�[�^�ʐM���I�����܂����H", Me.Text, MessageBoxButtons.OKCancel, MessageBoxIcon.Question) = Windows.Forms.DialogResult.Cancel Then
            Exit Sub
        End If

        '�X���b�h���I������
        jobsts = False
        mThread.Join()
    End Sub
End Class
