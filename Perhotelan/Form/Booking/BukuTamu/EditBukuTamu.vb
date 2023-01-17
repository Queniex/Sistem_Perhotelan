﻿Imports MySql.Data.MySqlClient
Public Class EditBukuTamu
    Dim status As String
    Dim kamar_lama As String

    Public Sub New()

        ' This call is required by the designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
        CbCheckin.Text = BookingKamar_.selectedTableBookingNoKamar.ToString()
        CbTamu.Text = BookingKamar_.selectedTableBookingNamaTamu.ToString()
        DtpCheckIn.Value = BookingKamar_.booking.GSCheckIn.ToString()
        DtpCheckOut.Value = BookingKamar_.booking.GSCheckOut.ToString()
        CbStatusKamar.Text = BookingKamar_.booking.GSStatus.ToString()
        LblTotalBayar.Text = BukuTamu_.hargaTotal.ToString()
    End Sub

    Private Sub BtnEditBukuTamu_Click(sender As Object, e As EventArgs) Handles BtnEditBukuTamu.Click
        If BukuTamu_.hargaTotal > 0 Then
            kamar_lama = BookingKamar_.selectedTableBookingNoKamar.ToString()
            Dim cek = BookingKamar_.booking.CheckTxtTamu(CbTamu.Text)
            Dim count_ = cek.Count

            If count_ > 0 Then

                If CbCheckin.Text <> kamar_lama Then
                    status = "Belum Terisi"
                    BookingKamar_.booking.UpdateDataStatusKamarById(BookingKamar_.booking.getIdKamarByNoKamar(kamar_lama), status)
                End If

                BookingKamar_.booking.GSCheckIn = DtpCheckIn.Value.ToString("yyyy/MM/dd")
                BookingKamar_.booking.GSCheckOut = DtpCheckOut.Value.ToString("yyyy/MM/dd")
                BookingKamar_.booking.GSIdTamu = BookingKamar_.booking.getIdTamuByNama(CbTamu.Text)
                BookingKamar_.booking.GSIdKamar = BookingKamar_.booking.getIdKamarByNoKamar(CbCheckin.Text)
                BookingKamar_.booking.GSStatus = CbStatusKamar.Text.ToString()

                BookingKamar_.booking.UpdateDataBookingById(BookingKamar_.selectedTableBookingIdBooking,
                                   BookingKamar_.booking.GSIdTamu,
                                   BookingKamar_.booking.GSIdKamar,
                                   BookingKamar_.booking.GSCheckIn,
                                   BookingKamar_.booking.GSCheckOut,
                                   BookingKamar_.booking.GSStatus)

                If String.Compare(BookingKamar_.booking.GSStatus, "Check In") = 0 Then
                    status = "Terisi"
                    BookingKamar_.booking.UpdateDataStatusKamarById(BookingKamar_.booking.GSIdKamar, status)
                ElseIf String.Compare(BookingKamar_.booking.GSStatus, "Check Out") = 0 Then
                    status = "Belum Terisi"
                    BookingKamar_.booking.UpdateDataStatusKamarById(BookingKamar_.booking.GSIdKamar, status)
                ElseIf String.Compare(BookingKamar_.booking.GSStatus, "Reserved") = 0 Then
                    status = "Terisi"
                    BookingKamar_.booking.UpdateDataStatusKamarById(BookingKamar_.booking.GSIdKamar, status)
                End If

                CbStatusKamar.Text = "Reserved"
                LblTotalBayar.Text = "Total Bayar"
                DtpCheckIn.Value = DateTime.Today
                DtpCheckOut.Value = DateTime.Today

                Dim Info = New InfoBukuTamu()
                Info.Show()
                Me.Close()
            Else
                MessageBox.Show("Data Tamu Yang Anda Masukkan Salah")
            End If
        Else
            MessageBox.Show("Tanggal Tidak Valid")
        End If
    End Sub

    Private Sub TxtBoxNoBooking_KeyPress(sender As Object, e As KeyPressEventArgs)
        If Not Char.IsNumber(e.KeyChar) And Not e.KeyChar = Chr(Keys.Delete) And Not e.KeyChar = Chr(Keys.Back) Then
            e.Handled = True
            MessageBox.Show("Hanya Dapat Memasukkan Angka!")
        End If
    End Sub

    Private Sub TxtNamaTamu_KeyPress(sender As Object, e As KeyPressEventArgs)
        If Not Char.IsLetter(e.KeyChar) And Not e.KeyChar = Chr(Keys.Delete) And Not e.KeyChar = Chr(Keys.Back) And Not e.KeyChar = Chr(Keys.Space) Then
            e.Handled = True
            MessageBox.Show("Karakter Tidak Diketahui")
        End If
    End Sub

    Private Sub CbCheckin_TextChanged(sender As Object, e As EventArgs) Handles CbCheckin.TextChanged
        LblTotalBayar.Text = "Masukkan Ulang Tanggal"
        Dim chk = BookingKamar_.booking.CheckTxtKamar2(BookingKamar_.textnokamar)
        Dim count = chk.Count

        If count > 0 Then
            BookingKamar_.harga = chk(2)
        End If
    End Sub

    Private Sub DtpCheckOut_ValueChanged(sender As Object, e As EventArgs) Handles DtpCheckOut.ValueChanged
        BookingKamar_.textnokamar = CbCheckin.Text
        Dim chk = BookingKamar_.booking.CheckTxtKamar2(BookingKamar_.textnokamar)
        Dim count = chk.Count

        If count > 0 Then
            BookingKamar_.harga = chk(2)
        End If

        If DtpCheckOut.Value.Year > DateTime.Today.Year Then

            BukuTamu_.hargaTotal = BookingKamar_.getDay(DtpCheckIn.Value.Date, DtpCheckOut.Value.Date, BookingKamar_.harga).ToString()
            LblTotalBayar.Text = "Rp " + BukuTamu_.hargaTotal
        ElseIf DtpCheckOut.Value.Year = DateTime.Today.Year Then

            If DtpCheckOut.Value.Month > DateTime.Today.Month Then
                BukuTamu_.hargaTotal = BookingKamar_.getDay(DtpCheckIn.Value.Date, DtpCheckOut.Value.Date, BookingKamar_.harga).ToString()
                LblTotalBayar.Text = "Rp " + BukuTamu_.hargaTotal
            ElseIf DtpCheckOut.Value.Month = DateTime.Today.Month Then

                If DtpCheckOut.Value.Day > DateTime.Today.Day Then
                    BukuTamu_.hargaTotal = BookingKamar_.getDay(DtpCheckIn.Value.Date, DtpCheckOut.Value.Date, BookingKamar_.harga).ToString()
                    LblTotalBayar.Text = "Rp " + BukuTamu_.hargaTotal
                Else
                    LblTotalBayar.Text = 0
                End If

            ElseIf DtpCheckOut.Value.Month < DateTime.Today.Month Then
                LblTotalBayar.Text = 0
            End If

        ElseIf DtpCheckOut.Value.Year < DateTime.Today.Year Then
            LblTotalBayar.Text = 0
        End If
    End Sub

    Private Sub BtnBatalKamar_Click(sender As Object, e As EventArgs) Handles BtnBatalKamar.Click
        BookingKamar_.selectedTableBookingNoKamar = ""
        BookingKamar_.selectedTableBookingNamaTamu = ""
        BookingKamar_.textnamatamu = ""
        BookingKamar_.textnokamar = ""

        Dim bk As New BukuTamu_(Login.Users.GSUserName, Login.Users.GSEmail)
        bk.Show()
        Me.Close()
    End Sub

    Private Sub EditBukuTamu_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Try
            Dim conn As New MySqlConnection("Data Source=localhost;user id=root;password=;database=projek_uas;Convert Zero Datetime=True;Allow Zero Datetime=True")
            Dim cmd2 As New MySqlCommand("SELECT nama FROM tamu", conn)
            Dim da2 As New MySqlDataAdapter(cmd2)
            Dim dt As New DataTable()
            Dim dt2 As New DataTable()
            da2.Fill(dt2)
            CbTamu.DataSource = dt2
            CbTamu.ValueMember = "nama"
            CbTamu.DisplayMember = "nama"
        Catch ex As Exception
            MsgBox("Error : " + ex.Message)
        Finally
            Class_Kamar.dbConn.Close()
        End Try
    End Sub

    Private Sub EditBukuTamu_Activated(sender As Object, e As EventArgs) Handles Me.Activated
        BookingKamar_.datas = BookingKamar_.booking.getKamar(BookingKamar_.selectedTableBookingNoKamar)
        CbCheckin.Items.Clear()
        For Each kamar In BookingKamar_.datas
            CbCheckin.Items.Add(kamar)
        Next
        BookingKamar_.datas.Clear()
    End Sub
End Class