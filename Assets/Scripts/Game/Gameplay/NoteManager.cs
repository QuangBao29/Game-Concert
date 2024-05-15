using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoteManager : SingletonMono<NoteManager>
{
    //expect to contain target zone data (or get target zone data) and spawn note due to that.
    [SerializeField]
    private SongManager _songManager = null;
    [SerializeField]
    private List<Note> _listNotePrefabs = null;
    [SerializeField]
    private GameObject _noteContainer = null;


    public Note OnSpawnNotesToTarget(Vector3 startPos, Vector3 endPos, Vector3 hitPos)
    {
        var idx = Random.Range(0, _listNotePrefabs.Count);
        var note = GCUtils.InstantiateObject<Note>(_listNotePrefabs[idx], 
            _noteContainer.transform);
        note.StartPos = startPos;
        note.EndPos = endPos;
        note.HitPos = hitPos;
        note.transform.position = startPos;
        note.transform.rotation = _listNotePrefabs[idx].transform.rotation;
        note.transform.localScale = _listNotePrefabs[idx].transform.localScale;
        note.gameObject.SetActive(true);
        return note;
    }
}
