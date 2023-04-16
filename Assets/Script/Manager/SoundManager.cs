using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    //ȫ����������Դ������Ϊһ����̬����
    private const string Sounds_Path_Prefix = "Sounds/";
    private const string bg = "Floating_light";

    //�������
    private AudioSource bgAudioSource;//���ڿ��Ʊ�������

    /// <summary>
    /// ���췽�����������Ӹ�����ʹ��
    /// </summary>
    /// <param name="facade"></param>
    public SoundManager() { }



    /// <summary>
    /// ������Դ·������������Դ
    /// </summary>
    private AudioClip LoadSound(string soundPath)
    {
        return Resources.Load<AudioClip>(Sounds_Path_Prefix + soundPath);
    }

    /// <summary>
    /// ��������
    /// </summary>
    /// <param name="audioSource"></param>��ƵԴ����������þ������ڲ�����Ƶ������AudioClip����Դ��
    /// <param name="audioClip"></param>����
    /// <param name="volume"></param>������С
    /// <param name="loop"></param>�Ƿ�ѭ��
    private void PlaySound(AudioSource audioSource, AudioClip audioClip, float volume, bool loop = false)
    {

        audioSource.clip = audioClip;//��������
        audioSource.volume = volume;//��������
        audioSource.loop = loop;//ѭ������
        audioSource.Play();//����

    }


    private void Start()
    {
        //����һ����Ϸ�������ڲ�������
        GameObject audioSourceGameObject = new GameObject("AudioSource(GameObject)");
        //��������������������audiosource��ƵԴ����������þ������ڲ�����Ƶ������AudioClip����Դ��
        bgAudioSource = audioSourceGameObject.AddComponent<AudioSource>();
        //����Ĭ�ϵ�����(����)
        PlaySound(bgAudioSource, LoadSound(bg), 0.02f, true);
    }
}
