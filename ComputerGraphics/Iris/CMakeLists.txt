# CMakeList.txt: Iris 的 CMake 项目，在此处包括源代码并定义
# 项目特定的逻辑。
#
cmake_minimum_required (VERSION 3.8)

project ("Iris")
set(CMAKE_MFC_FLAG 1)  
add_definitions(-DUNICODE -D_UNICODE)
add_definitions(-D_AFXDLL)
# 将源代码添加到此项目的可执行文件。
file(GLOB_RECURSE SOURCE_0 "include/*.h")
file(GLOB_RECURSE SOURCE_1 "src/*.cpp")
add_executable (Iris "Iris.cpp" "Iris.h" ${SOURCE_0} ${SOURCE_1})

# TODO: 如有需要，请添加测试并安装目标。
