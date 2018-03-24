Param(
    [string]$sourcesDirectory
)

cd ($sourcesDirectory + "\BuildTools\OpenCover");
OpenCover.Console.exe -target:"C:/Program Files/dotnet/dotnet.exe" -targetargs:"test ..\..\MisturTee.TestMeFool\MisturTee.TestMeFool.csproj --configuration Debug --no-build" -filter:"+[*]* -[*.Test*]*" -oldStyle -register:user -output:"OpenCover.xml";

cd ..;

.\OpenCoverToCoberturaConverter.exe -Wait -NoNewWindow -ArgumentList "-input:'..\OpenCover.xml'" -output:"..\Cobertura.xml" -sources:"..\";