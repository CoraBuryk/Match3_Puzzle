using UnityEngine;
using UnityEngine.U2D;
using UnityEngine.UI;

namespace Assets.Match.Scripts.UI
{

    public class SpriteFromAtlas : MonoBehaviour
    {

        [SerializeField] private SpriteAtlas _atlas;
        [SerializeField] private string _spriteName;

        void Start()
        {
            GetComponent<Image>().sprite = _atlas.GetSprite(_spriteName);
        }
    }
}