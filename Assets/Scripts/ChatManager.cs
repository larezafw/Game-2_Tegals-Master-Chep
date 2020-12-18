using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using ExitGames.Client.Photon;
using Photon.Pun;

public class ChatManager : MonoBehaviour
{
    StringQueue chatQueue;
    string[] allChat;

    public TextMeshProUGUI[] AllChatText;
    public TextMeshProUGUI InputChatText;

    private void Start()
    {
        chatQueue = new StringQueue(3);
        allChat = new string[3];
        GameObject.FindGameObjectWithTag(KeyWord.GAME_MANAGER)
            .GetComponent<GameManager>().SendChatMessege += GetChat;
    }

    public void SendChatButton()
    {
        if (DigitMeet(InputChatText.text))
        {
            ExitGames.Client.Photon.Hashtable chat = new ExitGames.Client.Photon.Hashtable();
            chat.Add(KeyWord.CHAT, InputChatText.text);
            PhotonNetwork.LocalPlayer.SetCustomProperties(chat);
        }
        else GetChat(KeyWord.RED_COLOR_TAG + "(Invalid. Maksimal 16 huruf!)" + KeyWord.CLOSE_COLOR_TAG);
        InputChatText.SetText("");
    }

    void GetChat(string chatText)
    {
        chatQueue.Enqueue(chatText);
        allChat = chatQueue.GetArrayString();
        for(int i=0; i < AllChatText.Length; i++)
            AllChatText[i].SetText(allChat[i]);   
    }

    bool DigitMeet(string text)
    {
        return text.Length <= 16 && text != "" && text != "null";
    }
}

public class StringQueue
{
    int itemLimit;
    Queue<string> queue;
    public StringQueue(int limit)
    {
        itemLimit = limit;
        queue = new Queue<string>(limit);
    }
    public void Enqueue(string item)
    {
        if (queue.Count >= itemLimit)
            queue.Dequeue();
        queue.Enqueue(item);
    }
    public string[] GetArrayString()
    {
        string[] Result = new string[3] { "", "", "" };

        foreach(string item in queue)
        {
            string storedResult = "null";
            int i=0;
            while (i < Result.Length) 
            {
                if (Result[i] != "")
                {
                    if (storedResult == "null")
                    {
                        storedResult = Result[i];
                        Result[i] = item;
                    }
                    else
                    {
                        string newResult = storedResult;
                        storedResult = Result[i];
                        Result[i] = newResult;
                    }
                    i++;
                }
                else
                {
                    if (storedResult == "null") Result[i] = item;
                    else Result[i] = storedResult;
                    i = 3;
                }
            }
        }
        return Result;
    }
}
