using System.Diagnostics;

var cts = new CancellationTokenSource();
cts.CancelAfter(3000);
var token = cts.Token;
;

var sw = Stopwatch.StartNew();
try {

	await Task.Delay(10000, token);

}
catch (TaskCanceledException e) {
	Console.WriteLine(e.Message);
}
finally {
	cts.Dispose();
}
Console.WriteLine($"{sw.ElapsedMilliseconds}");




