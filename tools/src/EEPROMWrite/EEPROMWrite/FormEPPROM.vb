'
' arduino Mega2560用EEPROMデータ書き込みツール
' 最終修正日 2014/02/28 v2.0
' 注意事項: 要arduino側に専用ソフトのインストール 
'

Imports System.Threading
Public Class FormEPPROM

    Dim datafpath As String     'データファイルパス
    Dim addr As UInt32          'データ書き込み先頭アドレス
    Dim flgActive As Boolean    '転送中フラグ(0:未転送 1:転送中)
    Dim mThread As Thread       'データ転送用スレッド
    Dim jobsts As Boolean       'スレッド稼働許可

    'Delegate Sub AddDataDelegate(ByVal str As String)

    'データファイルの指定
    Private Sub BtnFopen_Click(sender As Object, e As EventArgs) Handles BtnFopen.Click
        OpenFileDialogData.ShowDialog()
        datafpath = OpenFileDialogData.FileName
        TB_Fname.Text = datafpath
    End Sub

    'ログメッセージの書き込み
    Private Sub addLog(ByVal str As String)
        Tb_message.Text = Tb_message.Text + str + vbCrLf
    End Sub

    'コントロール操作の制限
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


    'データ書き込み処理
    '　バックグランド用スレッドから呼び出される
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

        '転送中状態に移行
        flgActive = True
        Me.BeginInvoke( _
        Sub()
            addLog("バックグラウンドで通信処理を開始しました.")
            ctrlSetting()
        End Sub)


        '** 書込み用データのロード **
        Try
            data = My.Computer.FileSystem.ReadAllBytes(datafpath)
        Catch ex As Exception
            Me.BeginInvoke( _
                Sub()
                    MessageBox.Show(ex.Message, Me.Text, MessageBoxButtons.OK, MessageBoxIcon.Error)
                    addLog("データファイルを読み込みに失敗しました.処理を終了します.")
                End Sub)
            flgActive = False
            Exit Sub
        End Try
        Me.BeginInvoke( _
            Sub()
                addLog("データファイルを読み込みました. サイズ:" & data.Length)
            End Sub)

        '** 書き込み開始アドレス送信,データサイズ 6バイト送信
        sdt(0) = (addr >> 16) And &HFF
        sdt(1) = (addr >> 8) And &HFF
        sdt(2) = addr And &HFF
        sdt(3) = (data.Length >> 16) And &HFF
        sdt(4) = (data.Length >> 8) And &HFF
        sdt(5) = data.Length And &HFF

        Try
            'コマンド送信
            SerialPort1.Write("d")
            SerialPort1.Write(sdt, 0, 6)
        Catch ex As Exception
            Me.BeginInvoke( _
                Sub()
                    MessageBox.Show(ex.Message, Me.Text, MessageBoxButtons.OK, MessageBoxIcon.Error)
                    addLog("開始アドレス送信,データサイズの送信に失敗しました.")
                End Sub)
            flgActive = False
            Exit Sub
        End Try

        '応答受信
        Try
            rcv = SerialPort1.ReadByte()
        Catch ex As Exception
            Me.BeginInvoke( _
                Sub()
                    MessageBox.Show(ex.Message, Me.Text, MessageBoxButtons.OK, MessageBoxIcon.Error)
                    addLog("開始アドレス送信,データサイズの送信の応答受信に失敗しました.")
                End Sub)
            flgActive = False
            Exit Sub
        End Try
        If rcv <> Asc("a") Then
            Me.BeginInvoke( _
                Sub()
                    addLog("開始アドレス送信,データサイズの送信の応答受信(NAK)に失敗しました.")
                End Sub)
            flgActive = False
            Exit Sub
        End If

        cnt = 0
        Me.BeginInvoke( _
            Sub()
                addLog("開始アドレス送信,データサイズの送信完了.")
                addLog("データ送信開始")
                Lbl_status.Text = "状況: 通信中 [" & cnt & "/" & data.Length & "]"
                Lbl_status.Update()
            End Sub)

        '** 以降データ送信処理
        ' １バイト送信の都度、応答を受信している
        For Each d As Byte In data

            '1バイト送信
            wdt(0) = d
            SerialPort1.Write(wdt, 0, 1)

            cksum = cksum + d
            cksum = cksum And &HFFFF

            '応答受信
            rcv = SerialPort1.ReadByte()

            '応答判定
            If rcv = Asc("o") Then
                '完了通知受信
                cnt = cnt + 1
                Me.BeginInvoke( _
                Sub()
                    addLog("データ送信完了.")
                    Lbl_status.Text = "状況: 転送完了 [" & cnt & "/" & data.Length & "]"
                    Lbl_status.Update()
                End Sub)
                flgend = True
                Exit For
            End If
            If rcv <> Asc(".") Then
                '書込み失敗
                Me.BeginInvoke( _
                Sub()
                    addLog("EEPROMの書き込みに失敗.")
                End Sub)
                Exit For
            End If

            '進捗表示
            cnt = cnt + 1
            If cnt Mod 100 = 0 Then
                Me.BeginInvoke( _
                Sub()
                    Lbl_status.Text = "状況: 通信中 [" & cnt & "/" & data.Length & "]"
                    Lbl_status.Update()
                End Sub)
            End If

            If jobsts = False Then
                Me.BeginInvoke( _
                Sub()
                    addLog("通信を中止しました.")
                    Lbl_status.Text = "状況: 中止 [" & cnt & "/" & data.Length & "]"
                    Lbl_status.Update()
                End Sub)
                Exit For
            End If
        Next

        '通信終了
        If flgend = True Then
            'データ送信正常終了
            Dim sum_l As Integer
            Dim sum_h As Integer
            Dim rcv_sum As Integer

            'チェックサム値取得要求
            SerialPort1.Write("s")
            sum_l = SerialPort1.ReadByte()
            sum_h = SerialPort1.ReadByte()
            rcv_sum = (sum_h << 8) + sum_l

            '双方のチェックサムの比較
            Dim msg As String
            If cksum <> rcv_sum Then
                msg = "チェックサム値が一致しません. 自:" & Hex(cksum) & " 他:" & Hex(rcv_sum)
            Else
                msg = "チェックサム値が一致しました. 自:" & Hex(cksum) & " 他:" & Hex(rcv_sum)
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
                addLog("シリアル接続を切断しました.")
                ctrlSetting()
            End Sub)
        End If

        '非通信状態に移行
        flgActive = False
        Me.BeginInvoke( _
        Sub()
            addLog("バックグラウンド処理を終了しました.")
            ctrlSetting()
        End Sub)

    End Sub

    'データ書き込みボタン
    Private Sub BtnWriteData_Click(sender As Object, e As EventArgs) Handles BtnWriteData.Click

        'ログのクリア
        Tb_message.Text = ""

        'シリアル接続
        Try
            SerialPort1.PortName = Cmb_comport.Text
            If SerialPort1.IsOpen = True Then
                MessageBox.Show("すでに" & SerialPort1.PortName & "は接続されています。", "エラー", MessageBoxButtons.OK, MessageBoxIcon.Error)
            End If
            Call SerialPort1.Open()
        Catch ex As Exception
            MessageBox.Show(ex.Message, "エラー", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
        addLog("シリアル接続しました.")

        'データファイルのチェック
        If datafpath = "" Then
            MessageBox.Show("データファイルが指定されていません", "エラー", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Exit Sub
        End If

        '書込み開始アドレスの取得
        addr = Convert.ToInt32(tb_addr.Text, 16)

        '書込み用スレットの実行
        flgActive = True
        jobsts = True
        ctrlSetting()
        mThread.Start() 'スレッドの開始
    End Sub

    'データファイルの入力変更を変数に反映させる
    Private Sub TB_Fname_TextChanged(sender As Object, e As EventArgs) Handles TB_Fname.TextChanged
        datafpath = TB_Fname.Text
    End Sub

    '書込み開始アドレス入力テキストボックスの入力制約指定
    Private Sub tb_addr_KeyPress(sender As Object, e As KeyPressEventArgs) Handles tb_addr.KeyPress
        If Asc(e.KeyChar) > 20 Then
            If InStr("0123456789ABCDEFabcdef", e.KeyChar) = 0 Then
                e.Handled = True
            End If
        End If
    End Sub

    'データ転送用のスレッド本体
    Private Sub job_thread()
        Upload()
    End Sub

    '[終了]ボタンを押した処理
    Private Sub Btn_end_Click(sender As Object, e As EventArgs) Handles Btn_end.Click
        Me.Close()
    End Sub

    'プログラム終了時の処理([終了]ボタン、[×]ボタン時)
    Private Sub FormEPPROM_FormClosing(sender As Object, e As FormClosingEventArgs) Handles Me.FormClosing
        If flgActive = True Then
            If MessageBox.Show("データ通信中ですが終了しますが？", Me.Text, MessageBoxButtons.OKCancel, MessageBoxIcon.Question) = Windows.Forms.DialogResult.Cancel Then
                e.Cancel = True
            End If
        End If
    End Sub

    'Formロード時の初期化処理
    Private Sub FormEPPROM_Load(sender As Object, e As EventArgs) Handles Me.Load

        'シリアルポートの一覧を取得しコンボボックスに表示する
        Dim ports() As String = IO.Ports.SerialPort.GetPortNames()
        For Each str As String In ports
            Cmb_comport.Items.Add(str)
            If ports.Length > 0 Then
                Cmb_comport.SelectedIndex = 0
            End If
        Next

        '操作ボタンの操作制約設定
        flgActive = False
        ctrlSetting()

        'データ転送用のスレッドの作成
        mThread = New Thread(AddressOf job_thread) 'バックグラウンド処理の設定
        mThread.IsBackground = True 'バックグラウンド・スレッドにする

    End Sub

    '[中止]ボタンによる中止処理
    Private Sub btn_stop_Click(sender As Object, e As EventArgs) Handles btn_stop.Click
        If MessageBox.Show("データ通信を終了しますか？", Me.Text, MessageBoxButtons.OKCancel, MessageBoxIcon.Question) = Windows.Forms.DialogResult.Cancel Then
            Exit Sub
        End If

        'スレッドを終了する
        jobsts = False
        mThread.Join()
    End Sub
End Class
