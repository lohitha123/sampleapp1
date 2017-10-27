using System;
using System.Threading.Tasks;

namespace Smartdocs
{
	public interface BarCodeScannerService
	{
		Task<string> ScanAsync();
	}
}

