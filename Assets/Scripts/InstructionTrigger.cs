using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class InstructionLine
{
    [TextArea(3, 10)]
    public string instruction;
    public List<Sprite> instructionImages = new List<Sprite>();
}

[System.Serializable]
public class Instruction
{
    public List<InstructionLine> instructionLines = new List<InstructionLine>();
}

public class InstructionTrigger : MonoBehaviour
{
    public Instruction instruction;

    public void TriggerInstruction()
    {
        InstructionManager.Instance.StartInstruction(instruction);
    }
}
