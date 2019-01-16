using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class InputManager : MonoBehaviour
{
    InputField inputField;
    private int opeCode;

    //InputFieldコンポーネントの取得および初期化メソッドの実行
    void Start()
    {
        inputField = GetComponent<InputField>();

        InitInputField();
    }

    //入力文字列を処理
    public void InputLogger()
    {
        string[] command = inputField.text.Split(' ');
        ProcessingCommand(command);
        InitInputField();
    }

    // InputFieldの初期化用メソッド
    void InitInputField()
    {
        // 値をリセット
        inputField.text = "";

        // フォーカス
        inputField.ActivateInputField();
    }

    private void ProcessingCommand(string[] command)
    {
        if (command.Length == 0) return;
        switch (command[0])
        {
            case "addInventory":
                for (int i = 0; i < int.Parse(command[2]); i++)
                    Player.instance.inventory.AddInventory(command[1]);
                break;
            case "decInventory":
                for (int i = 0; i < int.Parse(command[2]); i++)
                    Player.instance.inventory.DecInventory(command[1]);
                break;
            case "use":
                switch (command[1])
                {
                    case "item":
                        Player.instance.inventory.UseItem(command[2]);
                        break;
                    case "skill":
                        foreach(Skill skill in GameManager.instance.skillList)
                            if(skill.GetName() == command[2])
                            { skill.Use(Player.instance); break; }
                        break;
                    case "magic":
                        foreach (Magic magic in GameManager.instance.magicList)
                            if (magic.GetName() == command[2])
                            { magic.Use(Player.instance); break; }
                        break;
                    default:
                        Debug.Log("error: command 1");
                        break;
                }
                break;
            default:
                Debug.Log("error: command 0");
                break;
        }
    }
}