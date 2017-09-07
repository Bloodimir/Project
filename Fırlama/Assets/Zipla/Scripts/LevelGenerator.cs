using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class LevelGenerator : MonoBehaviour 
{
    public Transform startBlock;                        
    public Blocks[] blocksPool;                       
    public Offset blocksPositionOffset = new Offset();  
    public ChillBlocks chillBlocks;                    

    private Vector3 blockPos;
    private Transform playerT;
    private Block start;
    private Collider2D startCol;
    private int blockCounter;

	void Start () 
	{
        playerT = GameObject.FindGameObjectWithTag("Player").transform;
        start = startBlock.GetComponent<Block>();
        startCol = startBlock.GetComponent<Collider2D>();

        PlaceBlocks();
        InvokeRepeating("ReplaceBlocks", 1, 0.5F);
	}

    public void PlaceBlocks()
    {
        startCol.enabled = true;

        blockPos = startBlock.position + Vector3.right * Random.Range(blocksPositionOffset.min, blocksPositionOffset.max);

        blockCounter = 0;

        for(int i = 0; i < blocksPool.Length; i++)
        {
            blocksPool[i].blockTransform.position = blockPos;

            blocksPool[i].blockCollider.enabled = true;

            if (chillBlocks.enabled && Random.value < chillBlocks.chance / 100 && IsChill())
                blocksPool[i].block.SetChillBlock(true);
            else
                blocksPool[i].block.SetChillBlock(false);

            blockPos.x += Random.Range(blocksPositionOffset.min, blocksPositionOffset.max);

            blockCounter++;
        }
    }

    void ReplaceBlocks()
    {
        for(int i = 0; i < blocksPool.Length; i++)
        {
            if (blocksPool[i].blockTransform.position.x < playerT.position.x - 15)
            {
                blocksPool[i].blockTransform.position = blockPos;

                blocksPool[i].blockCollider.enabled = true;

                if (chillBlocks.enabled && Random.value < chillBlocks.chance / 100 && IsChill())
                    blocksPool[i].block.SetChillBlock(true);
                else
                    blocksPool[i].block.SetChillBlock(false);

                blockPos.x += Random.Range(blocksPositionOffset.min, blocksPositionOffset.max);
                blockCounter++;
            }
        }
    }

    bool IsChill()
    {
        return blockCounter > 0 && (blockCounter+1) % chillBlocks.density == 0;
    }

    public bool GetBlockStatus(Collider2D block)
    {
        if (!chillBlocks.enabled && block != startCol)
            return false;
        else if (block != startCol)
        {
            for (int i = 0; i < blocksPool.Length; i++)
                if (blocksPool[i].blockCollider == block)
                    return blocksPool[i].block.chillBlock;
            return false;
        }
        else
            return start.chillBlock;
    }
}

[System.Serializable]
public class Offset
{
    public float min = 5.5F;       
    public float max = 13.5F;       
}

[System.Serializable]
public class Blocks
{
    public Transform blockTransform;        
    public Block block;                    
    public PolygonCollider2D blockCollider;    
}

[System.Serializable]
public class ChillBlocks
{
    public bool enabled;                  
    [Range(1, 10)]
    public int density = 2;              
    [Range(1, 100)]
    public int chance = 10;               
}