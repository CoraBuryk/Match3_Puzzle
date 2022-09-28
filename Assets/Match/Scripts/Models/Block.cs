using Assets.Match.Scripts.Enum;
using UnityEngine;

namespace Assets.Match.Scripts.Models
{
    public class Block : MonoBehaviour
    {
        [SerializeField] protected BlockType _type;
        [SerializeField] public SpriteRenderer render { get; set; }


        public BlockType BlockType
        {
            get { return _type; }
        }

        private void Awake()
        {
            render = GetComponent<SpriteRenderer>();

        }

        public bool TryToMatchWithRenderer(Block tile)
        {
            if(!tile)
                return false;
            if (tile.GetComponent<SpriteRenderer>().sprite == render.sprite)
               return true;
            return TryToMatchWithType(tile);
        }

        public bool TryToMatchWithType(Block tile)
        {
            if (!tile)
                return false;
            if (tile.GetComponent<Block>().BlockType == BlockType)
                return true;
            return TryToMatchWithRenderer(tile);
        }

        public void SelectTile()
        {
            Debug.Log("selecte");
        }

        public void Unselect()
        {
            Debug.Log("unselecte");
        }

        public void DestroyBlock()
        {          
             GameObject.Destroy(gameObject);
        }
    }

}
