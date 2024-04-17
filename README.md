# Path of Exile C# SMD File Converter

Adapted from [Tzeentchful's transmute](https://github.com/Tzeentchful/transmute) written in GO.

This program converts Path of Exile's proprietary `.smd` 3D model file format to a generalized `.obj` format.

# Usage

This program can be run from the commandline and accepts two required arguments:
- `--smd <path_to_smd>` : `--smd` accpets a file path to the `.smd` file extracted from the `GGPK`
- `--obj <path_to_output_obj>` : `--obj` accepts a file path to the file to write the successfully converted `.obj`

Full example:
`PoeSmdConverter.exe --smd "C:\poe_model\rig_4182e167.smd" --obj "C:\poe_model\venarius.obj"`

# Limitations

There are several limitations that exist due to the lack of documentation of the `.smd` file format
- Due to differences in the `.smd` file formats for character models and for the MTX models, this program cannot convert the model files for individual MTX armorsets
- Due to the lack of documentation and the `.smd` file format being proprietary, it is subject to change without notice and newer `.smd` files may not convert properly
- This tool does not extract or convert:
	- Bones
	- Animations
	- Textures
	- Particles
	- etc.
- This tool only converts from `.smd` files and only into `.obj` files