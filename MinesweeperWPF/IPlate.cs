using System;

namespace Minesweeper.WPF
{
    interface IPlate
    {
        // Plate must have position getters
        int RowPosition { get; }
        int ColPosition { get; }
    }
}
