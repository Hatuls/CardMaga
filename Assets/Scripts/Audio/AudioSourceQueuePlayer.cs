public class AudioSourceQueuePlayer : AudioSourceAbstract {

    public override void OnEndPlayingSound()
    {
        base.OnEndPlayingSound();
        AudioManager.Instance.PlayNext();
    }
}
