Imports System.Net.Security
Imports System.Threading
Imports System.Threading.Tasks

Module Program
    Private ResultsTable(,) As Long
    Private BatchSize As Integer
    Private BatchNumber, NoOfIterations, NoOfCalculations As Long
    Private ThreadRandom As New Threading.ThreadLocal(Of Random)(Function() New Random(Guid.NewGuid().GetHashCode()))

    Sub Main()
        Console.ForegroundColor = ConsoleColor.Gray
        Console.WriteLine("Welcome to Alfie Kunz's Risk Calculator!" & vbCrLf)
        Console.Write("What is the max number of attacking pieces you want to calculate? (1-99) ")
        Dim NoOfAttackingPieces As Byte = Console.ReadLine()
        Console.Write("What is the max number of defending pieces you want to calculate? (1-99) ")
        Dim NoOfDefendingPieces As Byte = Console.ReadLine()
        ReDim ResultsTable(NoOfAttackingPieces, NoOfDefendingPieces)
        Console.WriteLine(vbCrLf & "The batch size determines the number of tests to perform in each scenario before the table is updated.")
        Console.Write("Please input your batch size (100 - 10,000 recommended): ")
        BatchSize = Console.ReadLine()
        Console.WriteLine(vbCrLf & "Thank you! Press any key to start processing...")
        Console.ReadLine()

        For n = 1 To NoOfAttackingPieces + 10
            Console.WriteLine()
        Next

        While Not Console.KeyAvailable
            Console.SetCursorPosition(0, Console.CursorTop - (8 + NoOfAttackingPieces))

            NoOfIterations += BatchSize * NoOfAttackingPieces * NoOfDefendingPieces
            BatchNumber += 1
            Parallel.For(1, NoOfAttackingPieces + 1, Sub(x)
                                                         For y = 1 To NoOfDefendingPieces
                                                             Dim CalculationResults = RunTableCalculations(CByte(x), CByte(y))
                                                             ResultsTable(x, y) += CalculationResults.Wins
                                                             Interlocked.Add(NoOfCalculations, CalculationResults.NoDiceRolles)
                                                         Next
                                                     End Sub)

            Console.WriteLine("Batch Number: " & BatchNumber.ToString("N0") & ".    Batch Size: " & BatchSize.ToString("N0") & ".    Number of battles simulated per scenario: " & (BatchNumber * BatchSize).ToString("N0") & ".")
            Console.WriteLine("Number of scenarios calculated: " & NoOfIterations.ToString("N0") & ".    Number of dice rolls simulated: " & NoOfCalculations.ToString("N0") & "." & vbCrLf)
            OutputTableToConsole(NoOfAttackingPieces, NoOfDefendingPieces)
            Console.WriteLine(vbCrLf & vbCrLf & "Press ENTER to stop calculating..." & vbCrLf)
        End While

        Console.WriteLine(vbCrLf & "Thank you for using this program. Goodbye!")
        Thread.Sleep(1000)
        Console.ReadLine()
    End Sub


    Function RunTableCalculations(ByVal AttackingPieces As Byte, ByVal DefendingPieces As Byte) As (Wins As Integer, NoDiceRolles As Integer)
        Dim AttackingDice(2) As Byte
        Dim DefendingDice(1) As Byte
        Dim NoOfAttackingWins As Integer
        Dim TempVar As Byte
        Dim TempAttackingPieces, TempDefendingPieces As Byte
        Dim NoDiceRolled As Integer = 0

        For n = 1 To BatchSize
            TempAttackingPieces = AttackingPieces
            TempDefendingPieces = DefendingPieces
            Do
                AttackingDice(0) = ThreadRandom.Value.Next(1, 7)
                If TempAttackingPieces > 1 Then
                    AttackingDice(1) = ThreadRandom.Value.Next(1, 7)
                    If TempAttackingPieces > 2 Then
                        AttackingDice(2) = ThreadRandom.Value.Next(1, 7)
                    Else
                        AttackingDice(2) = 0
                    End If
                Else
                    AttackingDice(1) = 0
                    AttackingDice(2) = 0
                End If
                DefendingDice(0) = ThreadRandom.Value.Next(1, 7)
                DefendingDice(1) = If(TempDefendingPieces > 1, ThreadRandom.Value.Next(1, 7), 0)

                NoDiceRolled += Math.Min(TempAttackingPieces, 3) + Math.Min(TempDefendingPieces, 2)

                'Sorts attacking dice, if needed.
                If AttackingDice(1) > AttackingDice(0) Then
                    TempVar = AttackingDice(0)
                    AttackingDice(0) = AttackingDice(1)
                    AttackingDice(1) = TempVar
                End If
                If AttackingDice(2) > AttackingDice(1) Then
                    TempVar = AttackingDice(1)
                    AttackingDice(1) = AttackingDice(2)
                    AttackingDice(2) = TempVar
                End If
                If AttackingDice(1) > AttackingDice(0) Then
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
                If AttackingDice(0) > DefendingDice(0) Then TempDefendingPieces -= 1 Else TempAttackingPieces -= 1
                If Not (AttackingDice(1) = 0 OrElse DefendingDice(1) = 0) Then
                    If AttackingDice(1) > DefendingDice(1) Then TempDefendingPieces -= 1 Else TempAttackingPieces -= 1
                End If

            Loop Until TempAttackingPieces = 0 OrElse TempDefendingPieces = 0
            If TempDefendingPieces = 0 Then NoOfAttackingWins += 1
        Next

        Return (NoOfAttackingWins, NoDiceRolled)
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
