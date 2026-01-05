# Pipboy2K
A Pip-boy 2000 MK VI recreation from Fallout 76, written in C# using RaylibCs as the underlying renderer. 

Greatly inspired by zapwizard's [pypboy](https://github.com/zapwizard/pypboy).
## Purpose
This is designed to be used on a physical Pip-boy prop that i have yet to make, that uses a Raspberry Pi SBC (or similiar) as the brains

This has been written as a "learning-as-you-go" type of experience, as i have little to no experence with Raylib. 

## Building
### Preface
Building with the `Release` configuration, will target AoT compilation. This may have some unforeseen consequences down the line.
### Windows & Linux (x86-64)
On x86-64 Windows and Linux, you should just be able to build as normal. As RaylibCs includes bindings for these.

    git clone https://github.com/Woljix/Pipboy2K.git
    cd Pipboy2K
    dotnet build -c Release

### Linux (arm64)

As RaylibCs does not include binding for arm64 with Linux (or Windows for that matter), this does not work out of the box with Raspberry Pi.

Inorder for that to work you need `libraylib.so` compiled for ARM64.

The easiest approach i just to get the source code for the version of Raylib that this project currently uses, which is [Raylib v5.5](https://github.com/raysan5/raylib/releases/tag/5.5) as of time of writing, and then compile it as a dynamic library.

**NOTE:** The following code will target `PLATFORM_DRM` which means that the game can render directly to the framebuffer instead of to X11/Wayland. If that is not desired, then `PLATFORM_DESKTOP` should be used instead 

    cd raylib5.5
    cd src
    make PLATFORM=PLATFORM_DRM RAYLIB_LIBTYPE=SHARED

This will produce a `libraylib.so` (and some other .so files, but use whichever) which can then be placed next to the Pipboy2K's executable. (There is probably a more Linux-friendly way of adding libraries - but this work for now) 

## TODO
* Improve `readme.md` with better explanation.
* Figure out how to make a build script that can clone and build raylib for linux-arm64 automatically.