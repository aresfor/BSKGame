@echo off
REM 获取当前脚本所在的目录
set "target_folder=%~dp0"

REM 去掉路径末尾的反斜杠
set "target_folder=%target_folder:~0,-1%"

REM 检查目标文件夹是否存在
if not exist "%target_folder%" (
    echo 无法找到目标文件夹: "%target_folder%"
    pause
    exit /b
)

echo 正在删除 "%target_folder%" 下的所有 .meta 文件...

REM 删除所有子文件夹及主文件夹中的 .meta 文件
for /r "%target_folder%" %%f in (*.meta) do (
    echo 正在删除文件: "%%f"
    del /f /q "%%f"
)

echo 删除完成！
pause
