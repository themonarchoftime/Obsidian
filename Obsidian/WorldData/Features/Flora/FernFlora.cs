﻿using Obsidian.WorldData.Generators;


namespace Obsidian.WorldData.Features.Flora;

public class FernFlora(GenHelper helper, IChunk chunk) : BaseFlora(helper, chunk, Material.Fern)
{
}
