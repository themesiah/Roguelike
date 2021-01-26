namespace Laresistance.Battle
{
    public class PlayerTargetSelection : ITargetSelection
    {
        private int currentSelectionInput = 0;
        public int GetTargetSelection()
        {
            int temp = currentSelectionInput;
            currentSelectionInput = 0;
            return temp;
        }

        public void DoTargetSelection(bool right)
        {
            currentSelectionInput = right ? 1 : -1;
        }
    }
}