using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public interface IQuestItemsData
{
    Sprite Icon(QuestItemType questItemType);
    QuestItem CreateQuestItem(QuestItemType questItemType);
}

