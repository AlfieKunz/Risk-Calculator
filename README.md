# Risk Battle Odds Calculator

![VB.NET](https://img.shields.io/badge/VB.NET-%23512BD4.svg?style=for-the-badge&logo=dotnet&logoColor=white)
![WinForms](https://img.shields.io/badge/Console_Application-%230078D4.svg?style=for-the-badge&logo=windows&logoColor=white)
![Pipeline](https://img.shields.io/badge/Pipeline-Monte%20Carlo%20Odds%20Simulator-%23B31B1B.svg?style=for-the-badge)
---

A simple program for simulating battles in the **board game RISK** (across a wide range of battle scenarios).

A 'battle' in RISK is determined by a set of dice rolls, where the attacking player has a maximum of 3 dice to roll (falling below this only if they don't have enough troops to attack with), and the defending player has a maximum of 2 dice to roll. After doing so, each player's dice are each ordered from highest to lowest, and are cross-compared into a maximum of 2 sets. The defender loses a troop for each set the attacker wins, and the attacker loses a troop for each set the defender wins **or draws**. This program continuously rolls dice until a full victory is achieved for either player. Crucially, "NoOfAttackingPieces" does not include the 1 attacking troop that must remain behind in order to 'hold' the country of attack, whilst this does not apply for the defending country (all troops are used in battle).

This work is self-motivated and self-funded, and is written primarily in VB.NET as a Visual Studio console application.

<p align="center">
  <img width="100%" alt="NNDoodle" src="./readme_img/RISK Odds.png" />
</p>

---

## Features and Highlights

✅ Application of standard RISK game logic, for handling dice rolls and win conditions, as well as excluding the 1 attacking troop to hold their country.  
✅ Simulates every attacker vs defender troop combination in a single run to construct a full odds matrix, whose entries become more accurate over time (convergent behaviour).  
✅ Outputs live results in a colourful, well-aligned gradient heat-map (where each colour represents the nature of the scenario results) to the console - self-refining.  
✅ Live computation of simulation statistics, including batches completed, total battles simulated, and total dice rolled.  
✅ Uses a single static Random instance, pre-allocated dice arrays, and a hand-unrolled sort to avoid per-iteration overhead across billions of rolls whilst ensuring unbiased Monte Carlo results.  
✅ Employs parallel processing to process ~300 million dice rolls per second.  

---

## Project Showcase

> **Project Demo:** You can see this project live directly through the [**project build**](https://drive.google.com/drive/folders/1I0Gy7R4WSeifpzXoMtCpzPegpZxljb_F?usp=sharing) (Intel 32/64-bit). Simply download and run the "Risk Calculator.exe" application.

Alternatively, one can download the source code, as instructed below, for full control.

> **Program Controls:**
>1) After opening and running the project solution in your IDE of choice, input values for the range of battles to simulate (ie: maximum number of attacking and defending troops to simulate - the program will iterate through all combinations of these numbers, from 1 to the maximum).
>2) Input the Mini Batch Size, which determines how many simulations to run (for each scenario) before updating the table of results (the larger NoOfAttackingPieces and NoOfDefendingPieces are, the lower this number should be).
>3) Hit "Run", and watch the table of results update in real-time!

---

## Installation and Folder Structure

### Required Software: Visual Studio (.NET 6.0).

To install, simply clone this repository using the following terminal prompts.
```bash
git clone https://github.com/AlfieKunz/Risk-Calculator
cd Risk-Calculator
```
Then, simply open the "Risk Calculator.sln" file in Visual Studio.

Feel free to also fork this repository, open an issue, or submit pull requests. All contributions welcome! :)  

---

## References & Inspiration

This work is self-motivated and self-funded. If you use this code or data in your work, please cite the associated preprint:

**Text Citation:**
> Kunz, A. (2023). *Risk Battle Odds Calculator*. Available at https://github.com/AlfieKunz/Risk-Calculator.

**BibTeX:**
```bibtex
@software{Kunz2023Risk,
  title = {Risk Battle Odds Calculator},
  author = {Kunz, Alfie},
  year = {2023},
  url = {https://github.com/AlfieKunz/Risk-Calculator}
}
```