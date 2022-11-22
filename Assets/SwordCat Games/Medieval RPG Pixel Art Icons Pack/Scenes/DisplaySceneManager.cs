using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.U2D;

public class DisplaySceneManager : MonoBehaviour
{
    [SerializeField] private GameObject hitPlayText;
    [SerializeField] private Transform groupHolder;
    [SerializeField] private GameObject iconPrefab;
    [SerializeField] private float iconsAmount;
    [SerializeField] private SpriteAtlas framesAtlas, iconsAtlas;

    private Sprite[] frameSprites, iconSprites;
    private List<Image> frames = new List<Image>(), icons = new List<Image>();

    private int currentFrame = -1, currentIcon = -1;

    private void Start() {
        if(hitPlayText != null)
            hitPlayText.SetActive(false);

        // Get sprites from atlas
        frameSprites = new Sprite[framesAtlas.spriteCount];
        framesAtlas.GetSprites(frameSprites);
        iconSprites = new Sprite[iconsAtlas.spriteCount];
        iconsAtlas.GetSprites(iconSprites);

        // Clear ui holder
        foreach(Transform child in groupHolder)
            Destroy(child.gameObject);

        // Instantiate all icons slots
        for(int i = 0; i<iconsAmount; i++){
            Transform auxTransform = Instantiate(iconPrefab, Vector3.zero, Quaternion.identity).transform;
            auxTransform.SetParent(groupHolder);
            auxTransform.localScale = Vector3.one;
            frames.Add(auxTransform.GetChild(0).GetComponent<Image>());
            icons.Add(auxTransform.GetChild(1).GetComponent<Image>());
        }

        randomize();
        

        
    }

    public void randomize(){
        randomFrame();
        randomIcon();
    }

    public void randomFrame(){
        foreach(Image frame in frames)
            frame.sprite = frameSprites[Random.Range(0,frameSprites.Length)];
    }

    public void randomIcon(){
        foreach(Image icon in icons)
            icon.sprite = iconSprites[Random.Range(0,iconSprites.Length)];
    }

    public void setFrame(){
        currentFrame++;
        if(currentFrame >= frameSprites.Length)
            currentFrame = 0;
        foreach(Image frame in frames)
            frame.sprite = frameSprites[currentFrame];
    }

    public void setIcon(){
        currentIcon++;
        if(currentIcon >= iconSprites.Length)
            currentIcon = 0;
        foreach(Image icon in icons)
            icon.sprite = iconSprites[currentIcon];
    }
}
