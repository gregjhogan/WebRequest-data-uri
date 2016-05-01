class DataWebRequest : System.Net.WebRequest
{
    [Uri] $uri

    DataWebRequest([Uri] $uri)
    {
        $this.uri = $uri
    }

    [System.Net.WebResponse] GetResponse()
    {
        return [DataWebResponse]::new($this.uri)
    }

    [IAsyncResult] BeginGetResponse([AsyncCallback] $callback, [object] $state)
    {
        $result = [DataUriAsyncResult]::new($state)
        $callback.Invoke($result)
        return $result
    }

    [System.Net.WebResponse] EndGetResponse([IAsyncResult] $asyncResult)
    {
        return $this.GetResponse()
    }
}

class DataWebResponse : System.Net.WebResponse
{
    [string] $mediatype = "text/plain"
    [System.Text.Encoding] $charset = [System.Text.Encoding]::ASCII
    [byte[]] $data

    DataWebResponse([Uri] $uri)
    {
        $parts = $uri.ToString() -match '(?s)data:(?<mediatype>[^;,]+/[^;,]+)?(?:;charset=(?<charset>[^;,]+))?(?<base64>;base64)?,(?<data>.*)'

        if ($Matches['mediatype'])
        {
            $this.mediatype = $Matches['mediatype']
        }
        
        if ($Matches['charset'])
        {
            $this.charset = [System.Text.Encoding]::GetEncoding($Matches['charset'])
        }

        if ($Matches['base64'])
        {
            $this.data = [System.Convert]::FromBase64String($Matches['data'])
        }
        else
        {
            $this.data = $this.charset.GetBytes($Matches['data'])
        }
    }

    [System.IO.Stream] GetResponseStream()
    {
        return [System.IO.MemoryStream]::new($this.data)
    }

    [long] get_ContentLength()
    {
        return $this.data.Length
    }

    [string] get_ContentType()
    {
        return $this.mediatype
    }
}

class DataUriAsyncResult : IAsyncResult
{
    [object] $state
    
    DataUriAsyncResult([object] $asyncState)
    {
        $this.state = $asyncState;
    }

    [object] get_AsyncState() { return $this.state } 

    [System.Threading.WaitHandle] get_AsyncWaitHandle() { return [System.Threading.AutoResetEvent]::new($true) }

    [bool] get_CompletedSynchronously() { return $true }

    [bool] get_IsCompleted() { return $true }
}

class DataWebRequestFactory : System.Net.IWebRequestCreate
{
    [System.Net.WebRequest] Create([Uri] $uri)
    {
        return [DataWebRequest]::new($uri)
    }
}

[System.Net.WebRequest]::RegisterPrefix("data", [DataWebRequestFactory]::new()) | Out-Null

$data = "Hello World!"
$data = [System.Convert]::ToBase64String([System.Text.Encoding]::UTF8.GetBytes($data))
$dataUri = [Uri]::new("data:text/plain;charset=ascii;base64,$data")

Write-Host $dataUri.ToString()
$value = [System.Net.WebClient]::new().DownloadString($dataUri)
Write-Host $value
