using System;
namespace Skrivmaskin.Core
{
    public sealed class VariableSubstitution : IBlockOfText
    {
        public VariableSubstitution ()
        {
        }

        public Variable Variable { get; set; }
    }
}
