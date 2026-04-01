using nsLogPrinter;

namespace CheckingDriveSpace
{
    public class CheckingDriveSpace
    {
        /// <summary>
        /// bytes 단위로 변환하기 위한 상수. 1GB = 1024 * 1024 * 1024 bytes
        /// 테스트테스트
        /// </summary>
        private const long _bytesPerGB = 1024 * 1024 * 1024;

        /// <summary>
        /// 사용가능한 디스크 용량이 몇퍼센트 남았는지 확인
        /// </summary>
        /// <param name="path"></param>
        /// <param name="driveSpace">최소한 남겨놓을 디스크 용량(GB)</param>
        /// <returns></returns>
        public bool CheckDriveSpace(string path, long driveSpace)
        {
            if (string.IsNullOrEmpty(path))
            {
                LogPrinter.lPrintf(LogLevel.MSG_ERROR_LEVEL, "The drive path to check does not exist.");
                return false;
            }

            if (driveSpace <= 0)
            {
                LogPrinter.lPrintf(LogLevel.MSG_ERROR_LEVEL, $"Invalid minimum free disk space setting: {driveSpace} GB.");
                return false;
            }

            DriveInfo driveInfo = new(path);

            // 전체 용량 (bytes)
            long totalSize = driveInfo.TotalSize;
            // 사용 가능한 용량 (bytes)
            long freeSpace = driveInfo.AvailableFreeSpace;
            // 사용한 용량 (bytes)
            long usedSpace = totalSize - freeSpace;
            LogPrinter.lPrintf(LogLevel.MSG_STATUS_LEVEL, $"Drive ({driveInfo.Name}): {usedSpace / _bytesPerGB} / {totalSize / _bytesPerGB} GB");

            #region 퍼센트 계산 방식 (GB -> %)
            //// 전체 디스크 용량 대비 설정된 최소 여유 디스크 용량이 몇퍼센트인지 계산
            //long driveSpaceBytes = driveSpace * _bytesPerGB;
            //double thresholdPer = (double)driveSpaceBytes / totalSize * 100;

            //double freeSpacePer = (double)freeSpace / totalSize * 100;
            //// 설정된 최소 여유 디스크 용량보다 현재 여유 디스크 용량이 적으면 true 반환
            //if (freeSpacePer <= thresholdPer)
            //{
            //    LogPrinter.lPrintf(LogLevel.MSG_WARNING_LEVEL, $"Drive {driveInfo.Name} has only {freeSpacePer}% free space.");
            //    return true;
            //}
            #endregion

            // 최소 디스크 용량을 bytes 단위로 변환
            long driveSpaceBytes = driveSpace * _bytesPerGB;

            // 설정된 최소 여유 디스크 용량보다 현재 여유 디스크 용량이 적으면 true 반환
            if (freeSpace <= driveSpaceBytes)
            {
                LogPrinter.lPrintf(LogLevel.MSG_WARNING_LEVEL, $"Drive {driveInfo.Name} has only {freeSpace / _bytesPerGB} GB free space.");
                return true;
            }

            return false;
        }
    }
}