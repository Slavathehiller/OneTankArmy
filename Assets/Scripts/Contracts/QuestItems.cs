using System;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public enum QuestItemType
{
    Undefined = -1,
    BoomFleaEgg = 0
}

public class QuestItemsData : IQuestItemsData
{
    private Dictionary<QuestItemType, Sprite> _sprites = new();
    private Dictionary<QuestItemType, GameObject> _prefabs = new();
    private Dictionary<QuestItemType, string> _iconPathes = new();
    private Dictionary<QuestItemType, string> _prefabPathes = new();

    protected readonly DiContainer _container;
    public QuestItemsData(DiContainer container)
    {
        _container = container ?? throw new ArgumentNullException(nameof(container));

        _iconPathes.Add(QuestItemType.BoomFleaEgg, "Sprites/BoomFleaEggsIcon");
        _prefabPathes.Add(QuestItemType.BoomFleaEgg, "Prefabs/QuestItems/fleaEggs");                
    }

    public Sprite Icon(QuestItemType questItemType)
    {
        if (!_sprites.ContainsKey(questItemType))
        {
            var sprite = Resources.Load<Sprite>(_iconPathes[questItemType]);
            _sprites.Add(questItemType, sprite);
            return sprite;
        }
        return _sprites[questItemType];
    }

    public QuestItem CreateQuestItem(QuestItemType questItemType)
    {
        if (!_prefabs.ContainsKey(questItemType))
        {
            var prefab = Resources.Load<GameObject>(_prefabPathes[questItemType]);
            _prefabs.Add(questItemType, prefab);
        }
        var instance = GameObject.Instantiate(_prefabs[questItemType]);
        _container.InjectGameObject(instance);
        return instance.GetComponent<QuestItem>();
    }
}

