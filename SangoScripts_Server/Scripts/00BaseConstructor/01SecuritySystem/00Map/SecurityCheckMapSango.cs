﻿using SangoUtils_Server_Scripts.Converter;

namespace SangoUtils_Server_Scripts.Security
{
    public static class SecurityCheckMapSango
    {
        private static bool CheckSignDataValid(string rawData, string signData, SecurityCheckServiceConfig config, SecuritySignConvertProtocol signConvertProtocol)
        {
            bool res = false;
            switch (config.signMethodCode)
            {
                case SignMethodCode.Md5:
                    res = Md5SignatureUtils.CheckMd5SignDataValid(rawData, signData, config.secretTimestamp, config.apiKey, config.apiSecret, config.checkLength, signConvertProtocol);
                    break;
            }
            return res;
        }
        private static bool CheckSignDataValid(long rawData, string signData, SecurityCheckServiceConfig config, SecuritySignConvertProtocol signConvertProtocol)
        {
            return CheckSignDataValid(rawData.ToString(), signData, config, signConvertProtocol);
        }

        public static void CheckProtocl_SIGNDATA(string registLimitTimestampNew, string signData, SecurityCheckServiceConfig config, Action<string> writeRegistInfoCallBack)
        {
            if (CheckSignDataValid(registLimitTimestampNew, signData, config, SecuritySignConvertProtocol.RawData))
            {
                writeRegistInfoCallBack?.Invoke(registLimitTimestampNew);
            }
            else
            {
                config.resultActionCallBack?.Invoke(RegistInfoCheckResult.UpdateFailed_SignError);
            }
        }

        public static void CheckProtocol_A_B_C_SIGN(string mixSignData, SecurityCheckServiceConfig config, Action<string> writeRegistInfoCallBack)
        {
            if (mixSignData.Length != 3 + config.checkLength)
            {
                config.resultActionCallBack?.Invoke(RegistInfoCheckResult.UpdateError_LenghthError);
                return;
            }
            int numYearPostNum = NumberUtilsSango.GetNumberFromNumberConvertProtocol(mixSignData[0], NumberConvertProtocol.ASCII_A0a26);
            if (numYearPostNum == -1)
            {
                config.resultActionCallBack?.Invoke(RegistInfoCheckResult.UpdateError_LenghthError);
                return;
            }
            int numYear = 2023 + numYearPostNum;
            int numMonth = NumberUtilsSango.GetNumberFromNumberConvertProtocol(mixSignData[1], NumberConvertProtocol.ASCII_A0a26);
            int numDay = NumberUtilsSango.GetNumberFromNumberConvertProtocol(mixSignData[2], NumberConvertProtocol.ASCII_A0a26);
            DateTime newRegistLimitDateTime = TimeUtils.GetDateTimeFromDateNumber(numYear, numMonth, numDay);
            if (newRegistLimitDateTime == DateTime.MinValue)
            {
                config.resultActionCallBack?.Invoke(RegistInfoCheckResult.UpdateError_SyntexError);
                return;
            }
            string md5DataStr = mixSignData.Substring(3, config.checkLength);
            long registLimitTimestampNew = TimeUtils.GetUnixDateTimeSeconds(newRegistLimitDateTime);
            if (CheckSignDataValid(registLimitTimestampNew, md5DataStr, config, SecuritySignConvertProtocol.AllToUpperChar))
            {
                writeRegistInfoCallBack?.Invoke(registLimitTimestampNew.ToString());
            }
            else
            {
                config.resultActionCallBack?.Invoke(RegistInfoCheckResult.UpdateFailed_SignError);
            }
        }
    }

    public enum RegistMixSignDataProtocol
    {
        SIGN,
        A_B_C_SIGN
    }
}
