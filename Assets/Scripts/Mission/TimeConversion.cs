public static class TimeConversion
{
    public static string RacingTime(float time)
    {
        var _Minutes = ((int)time / 60).ToString();
        var _SecondsN = (int)time % 60;
        var _Seconds = _SecondsN.ToString();
        if(_SecondsN < 10)
        {
            _Seconds = "0" + _SecondsN;
        }
        var _MilliSecondsN = ((int)(time * 100) % 100);
        var _MilliSeconds = ((int)(time * 100) % 100).ToString("");
        if (_MilliSecondsN < 10)
        {
            _MilliSeconds = "0" + _MilliSecondsN;
        }
        
        var str = _Minutes + " : " + _Seconds + " : " + _MilliSeconds;
        return str;
    }

}



