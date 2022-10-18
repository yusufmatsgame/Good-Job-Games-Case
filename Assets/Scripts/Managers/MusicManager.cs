
using UnityEngine;

public class MusicManager : MonoBehaviour
{
    private AudioSource source;
    public AudioClip[] tileSounds;

    public static MusicManager instance;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Debug.LogWarning("More than one Music Manager");
        }
        source = GetComponent<AudioSource>();
    }

    public void PlayTileSound(TileType tileType)
    {
        if (source != null)
            source.PlayOneShot(tileSounds[(int)tileType]);
    }


}
