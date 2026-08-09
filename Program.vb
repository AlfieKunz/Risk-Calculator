Imports System.Net.Security

Module Program
    Private ResultsTable(,) As Integer
    Private BatchSize As Integer
    Private BatchNumber, NoOfIterations, NoOfCalculations As UInt64

    Sub Main()
        Console.ForegroundColor = ConsoleColor.Gray
        Console.WriteLine("Welcome to Alfie Kunz's Risk Calculator!" & vbCrLf)
        Console.Write("What is the max number of attacking pieces you want to calculate? (1-99) ")
        Dim NoOfAttackingPieces As Byte = Console.ReadLine()
        Console.Write("What is the max number of defending pieces you want to calculate? (1-99) ")
        Dim NoOfDefendingPieces As Byte = Console.ReadLine()
        ReDim ResultsTable(NoOfAttackingPieces, NoOfDefendingPieces)
        Console.WriteLine(vbCrLf & "The batch size determines the number of tests to perform in each scenario before the table is updated.")
        Console.Write("Please input your batch size (10 - 10,000 recommended): ")
        BatchSize = Console.ReadLine()
        Console.WriteLine(vbCrLf & "Thank you! Press any key to start processing...")
        Console.ReadLine()

        For n = 1 To NoOfAttackingPieces + 10
            Console.WriteLine()
        Next

        While Not Console.KeyAvailable
            Console.SetCursorPosition(0, Console.CursorTop - (8 + NoOfAttackingPieces))

            BatchNumber += 1
            For x = 1 To NoOfAttackingPieces
                For y = 1 To NoOfDefendingPieces
                    ResultsTable(x, y) += RunTableCalculations(x, y)
                Next
            Next

            Console.WriteLine("Batch Number: " & BatchNumber.ToString("N0") & ".    Batch Size: " & BatchSize.ToString("N0") & ".    Number of battles simulated per scenario: " & (BatchNumber * BatchSize).ToString("N0") & ".")
            Console.WriteLine("Number of scenarios calculated: " & NoOfIterations.ToString("N0") & ".    Number of dice rolls simulated: " & NoOfCalculations.ToString("N0") & "." & vbCrLf)
            OutputTableToConsole(NoOfAttackingPieces, NoOfDefendingPieces)
            Console.WriteLine(vbCrLf & vbCrLf & "Press ENTER to stop calculating..." & vbCrLf)
        End While

        Console.WriteLine(vbCrLf & "Thank you for using this program. Goodbye!")
        Console.ReadLine()
    End Sub


    Private AttackingDice(2) As Byte
    Private DefendingDice(1) As Byte
    Function RunTableCalculations(ByVal AttackingPieces As Byte, ByVal DefendingPieces As Byte) As Integer
        Static RNDGen As New Random()
        Dim NoOfAttackingWins As Integer
        Dim TempVar As Byte
        Dim HasBeenSorted As Boolean
        Dim TempAttackingPieces, TempDefendingPieces As Byte

        For n = 1 To BatchSize
            NoOfIterations += 1
            TempAttackingPieces = AttackingPieces
            TempDefendingPieces = DefendingPieces
            Do
                For a = 0 To 2
                    If TempAttackingPieces > a Then
                        AttackingDice(a) = RNDGen.Next(1, 7)
                        NoOfCalculations += 1
                    Else
                        AttackingDice(a) = 0
                    End If
                Next
                For b = 0 To 1
                    If TempDefendingPieces > b Then
                        DefendingDice(b) = RNDGen.Next(1, 7)
                        NoOfCalculations += 1
                    Else
                        DefendingDice(b) = 0
                    End If
                Next

                'Sorts attacking dice, if needed.
                HasBeenSorted = True
                For d = 0 To 1
                    If AttackingDice(d + 1) > AttackingDice(d) Then
                        HasBeenSorted = False
                        TempVar = AttackingDice(d)
                        AttackingDice(d) = AttackingDice(d + 1)
                        AttackingDice(d + 1) = TempVar
                    End If
                Next
                If Not HasBeenSorted AndAlso AttackingDice(1) > AttackingDice(0) Then
                    TempVar = AttackingDice(0)
                    AttackingDice(0) = AttackingDice(1)
                    AttackingDice(1) = TempVar
                End If


                'Sorts defending dice, if needed.
                If DefendingDice(1) > DefendingDice(0) Then
                    TempVar = DefendingDice(0)
                    DefendingDice(0) = DefendingDice(1)
                    DefendingDice(1) = TempVar
                End If


                'Performs calculation.
                If Not (AttackingDice(0) = 0 OrElse DefendingDice(0) = 0) Then
                    If AttackingDice(0) > DefendingDice(0) Then TempDefendingPieces -= 1 Else TempAttackingPieces -= 1
                    If Not (AttackingDice(1) = 0 OrElse DefendingDice(1) = 0) Then
                        If AttackingDice(1) > DefendingDice(1) Then TempDefendingPieces -= 1 Else TempAttackingPieces -= 1
                    End If
                End If

            Loop Until TempAttackingPieces = 0 OrElse TempDefendingPieces = 0
            If TempDefendingPieces = 0 Then NoOfAttackingWins += 1
        Next

        Return NoOfAttackingWins
    End Function

    Private Sub OutputTableToConsole(ByVal NoOfAttackingPieces As Byte, ByVal NoOfDefendingPieces As Byte)
        Dim PercentageWins As Decimal
        Dim CorrectString As String
        Console.ForegroundColor = ConsoleColor.White
        Console.Write("       ")
        For n = 1 To NoOfDefendingPieces
            Console.Write(n & ":       ")
            If n < 10 Then Console.Write(" ")
        Next
        Console.WriteLine()
        For x = 1 To NoOfAttackingPieces
            If x < 10 Then Console.Write(" ")
            Console.Write(x & ":  ")
            For y = 1 To NoOfDefendingPieces
                PercentageWins = 100 * ResultsTable(x, y) / (BatchNumber * BatchSize)
                If PercentageWins > 55.0 Then
                    If PercentageWins >= 90.0 Then
                        Console.ForegroundColor = ConsoleColor.DarkYellow
                    ElseIf PercentageWins > 75.0 Then
                        Console.ForegroundColor = ConsoleColor.DarkGreen
                    ElseIf PercentageWins > 60 Then
                        Console.ForegroundColor = ConsoleColor.Green
                    Else
                        Console.ForegroundColor = ConsoleColor.Gray
                    End If
                ElseIf PercentageWins < 45.0 Then
                    If PercentageWins <= 25.0 Then
                        Console.ForegroundColor = ConsoleColor.DarkRed
                    ElseIf PercentageWins < 40 Then
                        Console.ForegroundColor = ConsoleColor.Red
                    Else
                        Console.ForegroundColor = ConsoleColor.Gray
                    End If
                Else
                    Console.ForegroundColor = ConsoleColor.White
                End If
                CorrectString = PercentageWins.ToString("00.00")
                If CorrectString = "100.00" Then CorrectString = "100.0"
                Console.Write(CorrectString & "%")
                If y < NoOfDefendingPieces Then Console.Write("    ")
            Next
            Console.ForegroundColor = ConsoleColor.White
            Console.WriteLine("  :" & x)
        Next
        Console.ForegroundColor = ConsoleColor.Gray
    End Sub




End Module
