/* Url: https://servicestack.net/dist/JsonServiceClient.swift
//
// JsonServiceClient.swift
// ServiceStackClient
//
// Copyright (c) 2015 ServiceStack LLC. All rights reserved.
// License: https://servicestack.net/terms
*/

import Foundation



public protocol IReturn
{
    typealias Return : JsonSerializable
}

public protocol IReturnVoid {}

public protocol IGet {}
public protocol IPost {}
public protocol IPut {}
public protocol IDelete {}
public protocol IPatch {}

public protocol ServiceClient
{
    func get<T : IReturn where T : JsonSerializable>(request:T) throws -> T.Return
    func get<T : IReturnVoid where T : JsonSerializable>(request:T) throws -> Void
    func get<T : IReturn where T : JsonSerializable>(request:T, query:[String:String]) throws -> T.Return
    func get<T : JsonSerializable>(relativeUrl:String) throws -> T
    func getAsync<T : IReturn where T : JsonSerializable>(request:T) -> Promise<T.Return>
    func getAsync<T : IReturnVoid where T : JsonSerializable>(request:T) -> Promise<Void>
    func getAsync<T : IReturn where T : JsonSerializable>(request:T, query:[String:String]) -> Promise<T.Return>
    func getAsync<T : JsonSerializable>(relativeUrl:String) -> Promise<T>

    func post<T : IReturn where T : JsonSerializable>(request:T) throws -> T.Return
    func post<T : IReturnVoid where T : JsonSerializable>(request:T) throws -> Void
    func post<Response : JsonSerializable, Request:JsonSerializable>(relativeUrl:String, request:Request?) throws -> Response
    func postAsync<T : IReturn where T : JsonSerializable>(request:T) -> Promise<T.Return>
    func postAsync<T : IReturnVoid where T : JsonSerializable>(request:T) -> Promise<Void>
    func postAsync<Response : JsonSerializable, Request:JsonSerializable>(relativeUrl:String, request:Request?) -> Promise<Response>

    func put<T : IReturn where T : JsonSerializable>(request:T) throws -> T.Return
    func put<T : IReturnVoid where T : JsonSerializable>(request:T) throws -> Void
    func put<Response : JsonSerializable, Request:JsonSerializable>(relativeUrl:String, request:Request?) throws -> Response
    func putAsync<T : IReturn where T : JsonSerializable>(request:T) -> Promise<T.Return>
    func putAsync<T : IReturnVoid where T : JsonSerializable>(request:T) -> Promise<Void>
    func putAsync<Response : JsonSerializable, Request:JsonSerializable>(relativeUrl:String, request:Request?) -> Promise<Response>

    func delete<T : IReturn where T : JsonSerializable>(request:T) throws -> T.Return
    func delete<T : IReturnVoid where T : JsonSerializable>(request:T) throws -> Void
    func delete<T : IReturn where T : JsonSerializable>(request:T, query:[String:String]) throws -> T.Return
    func delete<T : JsonSerializable>(relativeUrl:String) throws -> T
    func deleteAsync<T : IReturn where T : JsonSerializable>(request:T) -> Promise<T.Return>
    func deleteAsync<T : IReturnVoid where T : JsonSerializable>(request:T) -> Promise<Void>
    func deleteAsync<T : IReturn where T : JsonSerializable>(request:T, query:[String:String]) -> Promise<T.Return>
    func deleteAsync<T : JsonSerializable>(relativeUrl:String) -> Promise<T>

    func patch<T : IReturn where T : JsonSerializable>(request:T) throws -> T.Return
    func patch<T : IReturnVoid where T : JsonSerializable>(request:T) throws -> Void
    func patch<Response : JsonSerializable, Request:JsonSerializable>(relativeUrl:String, request:Request?) throws -> Response
    func patchAsync<T : IReturn where T : JsonSerializable>(request:T) -> Promise<T.Return>
    func patchAsync<T : IReturnVoid where T : JsonSerializable>(request:T) -> Promise<Void>
    func patchAsync<Response : JsonSerializable, Request:JsonSerializable>(relativeUrl:String, request:Request?) -> Promise<Response>

    func send<T : IReturn where T : JsonSerializable>(request:T) throws -> T.Return
    func send<T : IReturnVoid where T : JsonSerializable>(request:T) throws -> Void
    func send<T : JsonSerializable>(intoResponse:T, request:URLRequest) throws -> T
    func sendAsync<T : JsonSerializable>(intoResponse:T, request:URLRequest) -> Promise<T>

    func getData(url:String) throws -> NSData
    func getDataAsync(url:String) -> Promise<NSData>
}

public class JsonServiceClient : ServiceClient
{
    var baseUrl:String
    var replyUrl:String
    var domain:String
    var lastError:NSError?
    var lastTask:URLSessionDataTask?
    var onError:((NSError) -> Void)?
    var timeout:TimeInterval?
    var cachePolicy:NSURLRequest.CachePolicy = NSURLRequest.CachePolicy.reloadIgnoringLocalCacheData

    var requestFilter:((URLRequest) -> Void)?
    var responseFilter:((URLResponse) -> Void)?

    public struct Global
    {
        static var requestFilter:((URLRequest) -> Void)?
        static var responseFilter:((URLResponse) -> Void)?
        static var onError:((NSError) -> Void)?
    }

    public init(baseUrl:String)
    {
        self.baseUrl = baseUrl.hasSuffix("/") ? baseUrl : baseUrl + "/"
        self.replyUrl = self.baseUrl + "json/reply/"
        let url = NSURL(string: self.baseUrl)
        self.domain = url!.host!
    }

    func createSession() -> URLSession {
        let config = URLSessionConfiguration.default

        let session = URLSession(configuration: config)
        return session
    }

    func handleError(nsError:NSError) -> NSError {
        return fireErrorCallbacks(NSError(domain: domain, code: nsError.code,
            userInfo:["responseStatus": ["errorCode":nsError.code.toString(), "message":nsError.description]]))
    }

    func fireErrorCallbacks(_ error:NSError) -> NSError {
        lastError = error
        if onError != nil {
            onError!(error)
        }
        if Global.onError != nil {
            Global.onError!(error)
        }
        return error
    }

    func getItem(_ map:NSDictionary, key:String) -> AnyObject? {
        return map[String(key[0]).lowercaseString + key[1..<key.length]] ?? map[String(key[0]).uppercaseString + key[1..<key.length]]
    }

    func populateResponseStatusFields( errorInfo: inout [String : AnyObject], withObject:AnyObject) {
        if let status = getItem(withObject as! NSDictionary, key: "ResponseStatus") as? NSDictionary {
            if let errorCode = getItem(status, key: "errorCode") as? NSString {
                errorInfo["errorCode"] = errorCode
            }
            if let message = getItem(status, key: "message") as? NSString {
                errorInfo["message"] = message
            }
            if let stackTrace = getItem(status, key: "stackTrace") as? NSString {
                errorInfo["stackTrace"] = stackTrace
            }
            if let errors: AnyObject = getItem(status, key: "errors") {
                errorInfo["errors"] = errors
            }
        }
    }

    func handleResponse<T : JsonSerializable>(intoResponse:T, data:NSData, response:URLResponse, error:NSErrorPointer = nil) -> T? {
        if let nsResponse = response as? HTTPURLResponse {
            if nsResponse.statusCode >= 400 {
                var errorInfo = [String : AnyObject]()

                errorInfo["statusCode"] = nsResponse.statusCode as AnyObject?
                errorInfo["statusDescription"] = nsResponse.description as AnyObject?

                if let _ = nsResponse.allHeaderFields["Content-Type"] as? String {
                    if let obj:AnyObject = parseJsonBytes(data) {
                        errorInfo["response"] = obj
                        errorInfo["errorCode"] = nsResponse.statusCode.toString() as AnyObject?
                        errorInfo["message"] = nsResponse.statusDescription as AnyObject?
                        populateResponseStatusFields(&errorInfo, withObject:obj)
                    }
                }

                let ex = fireErrorCallbacks(NSError(domain:self.domain, code:nsResponse.statusCode, userInfo:errorInfo))
                if error != nil {
                    error.memory = ex
                }

                return nil
            }
        }

        if (intoResponse is ReturnVoid) {
            return intoResponse
        }

        if responseFilter != nil {
            responseFilter!(response)
        }
        if Global.responseFilter != nil {
            Global.responseFilter!(response)
        }

        if let json = NSString(data: data, encoding: NSUTF8StringEncoding) {
            if let dto = Type<T>.fromJson(intoResponse, json: json as String) {
                return dto
            }
        }
        return nil
    }

    public func createUrl<T : JsonSerializable>(dto:T, query:[String:String] = [:]) -> String {
        var requestUrl = self.replyUrl + T.typeName

        var sb = ""
        for pi in T.properties {
            if let strValue = pi.jsonValueAny(dto)?.stripQuotes() {
                sb += sb.length == 0 ? "?" : "&"
                sb += "\(pi.name.urlEncode()!)=\(strValue.urlEncode()!)"
            }
            else if let strValue = pi.stringValueAny(dto) {
                sb += sb.length == 0 ? "?" : "&"
                sb += "\(pi.name.urlEncode()!)=\(strValue.urlEncode()!)"
            }
        }

        for (key,value) in query {
            sb += sb.length == 0 ? "?" : "&"
            sb += "\(key)=\(value.urlEncode()!)"
        }

        requestUrl += sb

        return requestUrl
    }

    public func createRequest<T : JsonSerializable>(url:String, httpMethod:String, request:T? = nil) -> URLRequest {
        var contentType:String?
        var requestBody:NSData?

        if let dto = request {
            contentType = "application/json"
            requestBody = dto.toJson().dataUsingEncoding(NSUTF8StringEncoding)
        }

        return self.createRequest(url, httpMethod: httpMethod, requestType: contentType, requestBody: requestBody)
    }

    public func createRequest(url:String, httpMethod:String, requestType:String? = nil, requestBody:NSData? = nil) -> URLRequest {
        let nsUrl = NSURL(string: url)!

        let req = self.timeout == nil
            ? URLRequest(URL: nsUrl)
            : URLRequest(URL: nsUrl, cachePolicy: self.cachePolicy, timeoutInterval: self.timeout!)

        req.HTTPMethod = httpMethod
        req.setValue("application/json", forHTTPHeaderField: "Accept")

        req.HTTPBody = requestBody
        if let contentType = requestType {
            req.setValue(contentType, forHTTPHeaderField: "Content-Type")
        }

        if requestFilter != nil {
            requestFilter!(req)
        }

        if Global.requestFilter != nil {
            Global.requestFilter!(req)
        }

        return req
    }

    public func send<T : JsonSerializable>(intoResponse:T, request:URLRequest) throws -> T {
        var response:URLResponse? = nil

        var data = NSData()
        do {
            data = try NSURLConnection.sendSynchronousRequest(request, returningResponse: &response)
            var error:NSError? = NSError(domain: NSURLErrorDomain, code: NSURLErrorUnknown, userInfo: nil)
            if response == nil {
                if let e = error {
                    throw e
                }
                return T()
            }
            if let dto = self.handleResponse(intoResponse, data: data, response: response!, error: &error) {
                return dto
            }
            if let e = error {
                throw e
            }
            return T()
        } catch var ex as NSError? {
            if let r = response, let e = self.handleResponse(intoResponse, data: data, response: r, error: &ex) {
                return e
            }
            throw ex!
        }
    }

    public func sendAsync<T : JsonSerializable>(intoResponse:T, request:URLRequest) -> Promise<T> {

        return Promise<T> { (complete, reject) in

            let task = self.createSession().dataTaskWithRequest(request) { (data, response, error) in
                if error != nil {
                    reject(self.handleError(error!))
                }
                else {
                    var resposneError:NSError?
                    let response = self.handleResponse(intoResponse, data: data!, response: response!, error: &resposneError)
                    if resposneError != nil {
                        reject(self.fireErrorCallbacks(resposneError!))
                    }
                    else if let dto = response {
                        complete(dto)
                    } else {
                        complete(T()) //return empty dto in promise callbacks
                    }
                }
            }

            task.resume()
            self.lastTask = task
        }
    }

    func resolveUrl(relativeOrAbsoluteUrl:String) -> String {
        return relativeOrAbsoluteUrl.hasPrefix("http:")
            || relativeOrAbsoluteUrl.hasPrefix("https:")
            ? relativeOrAbsoluteUrl
            : baseUrl.combinePath(relativeOrAbsoluteUrl)
    }

    func hasRequestBody(httpMethod:String) -> Bool
    {
        switch httpMethod {
            case HttpMethods.Get, HttpMethods.Delete, HttpMethods.Head, HttpMethods.Options:
                return false
            default:
                return true
        }
    }

    func getSendMethod<T : JsonSerializable>(request:T) -> String {
        return request is IGet ?
             HttpMethods.Get
            : request is IPost ?
              HttpMethods.Post
            : request is IPut ?
              HttpMethods.Put
            : request is IDelete ?
              HttpMethods.Delete
            : request is IPatch ?
              HttpMethods.Patch :
              HttpMethods.Post;
    }

    public func send<T : IReturn where T : JsonSerializable>(request:T) throws -> T.Return {
        let httpMethod = getSendMethod(request)
        return hasRequestBody(httpMethod)
            ? try send(T.Return(), request: self.createRequest(replyUrl.combinePath(T.typeName), httpMethod:httpMethod, request:request))
            : try send(T.Return(), request: self.createRequest(self.createUrl(request), httpMethod:httpMethod))
    }

    public func send<T : IReturnVoid where T : JsonSerializable>(request:T) throws {
        let httpMethod = getSendMethod(request)
        if hasRequestBody(httpMethod) {
            try send(ReturnVoid.void, request: self.createRequest(replyUrl.combinePath(T.typeName), httpMethod:httpMethod, request:request))
        }
        else {
            try send(ReturnVoid.void, request: self.createRequest(self.createUrl(request), httpMethod:httpMethod))
        }
    }

    public func sendAsync<T : IReturn where T : JsonSerializable>(request:T) -> Promise<T.Return> {
        let httpMethod = getSendMethod(request)
        return hasRequestBody(httpMethod)
            ? sendAsync(T.Return(), request: self.createRequest(replyUrl.combinePath(T.typeName), httpMethod:httpMethod, request:request))
            : sendAsync(T.Return(), request: self.createRequest(self.createUrl(request), httpMethod:httpMethod))
    }

    public func sendAsync<T : IReturnVoid where T : JsonSerializable>(request:T) -> Promise<Void> {
        let httpMethod = getSendMethod(request)
        return hasRequestBody(httpMethod)
            ? sendAsync(ReturnVoid.void, request: self.createRequest(replyUrl.combinePath(T.typeName), httpMethod:HttpMethods.Post, request:request))
              .then({ x -> Void in })
            : sendAsync(ReturnVoid.void, request: self.createRequest(self.createUrl(request), httpMethod:HttpMethods.Get))
                .then({ x -> Void in })
    }


    public func get<T : IReturn where T : JsonSerializable>(request:T) throws -> T.Return {
        return try send(T.Return(), request: self.createRequest(self.createUrl(request), httpMethod:HttpMethods.Get))
    }

    public func get<T : IReturnVoid where T : JsonSerializable>(request:T) throws -> Void {
        try send(ReturnVoid.void, request: self.createRequest(self.createUrl(request), httpMethod:HttpMethods.Get))
    }

    public func get<T : IReturn where T : JsonSerializable>(request:T, query:[String:String]) throws -> T.Return {
        return try send(T.Return(), request: self.createRequest(self.createUrl(request, query:query), httpMethod:HttpMethods.Get))
    }

    public func get<T : JsonSerializable>(relativeUrl:String) throws -> T {
        return try send(T(), request: self.createRequest(resolveUrl(relativeUrl), httpMethod:HttpMethods.Get))
    }

    public func getAsync<T : IReturn where T : JsonSerializable>(request:T) -> Promise<T.Return> {
        return sendAsync(T.Return(), request: self.createRequest(self.createUrl(request), httpMethod:HttpMethods.Get))
    }

    public func getAsync<T : IReturnVoid where T : JsonSerializable>(request:T) -> Promise<Void> {
        return sendAsync(ReturnVoid.void, request: self.createRequest(self.createUrl(request), httpMethod:HttpMethods.Get))
            .then({ x -> Void in })
    }

    public func getAsync<T : IReturn where T : JsonSerializable>(request:T, query:[String:String]) -> Promise<T.Return> {
        return sendAsync(T.Return(), request: self.createRequest(self.createUrl(request, query:query), httpMethod:HttpMethods.Get))
    }

    public func getAsync<T : JsonSerializable>(relativeUrl:String) -> Promise<T> {
        return sendAsync(T(), request: self.createRequest(resolveUrl(relativeUrl), httpMethod:HttpMethods.Get))
    }


    public func post<T : IReturn where T : JsonSerializable>(request:T) throws -> T.Return {
        return try send(T.Return(), request: self.createRequest(replyUrl.combinePath(T.typeName), httpMethod:HttpMethods.Post, request:request))
    }

    public func post<T : IReturnVoid where T : JsonSerializable>(request:T) throws -> Void {
        try send(ReturnVoid.void, request: self.createRequest(replyUrl.combinePath(T.typeName), httpMethod:HttpMethods.Post, request:request))
    }

    public func post<Response : JsonSerializable, Request:JsonSerializable>(relativeUrl:String, request:Request?) throws -> Response {
        return try send(Response(), request: self.createRequest(resolveUrl(relativeUrl), httpMethod:HttpMethods.Post, request:request))
    }

    public func postAsync<T : IReturn where T : JsonSerializable>(request:T) -> Promise<T.Return> {
        return sendAsync(T.Return(), request: self.createRequest(replyUrl.combinePath(T.typeName), httpMethod:HttpMethods.Post, request:request))
    }

    public func postAsync<T : IReturnVoid where T : JsonSerializable>(request:T) -> Promise<Void> {
        return sendAsync(ReturnVoid.void, request: self.createRequest(replyUrl.combinePath(T.typeName), httpMethod:HttpMethods.Post, request:request))
            .then({ x -> Void in })
    }

    public func postAsync<Response : JsonSerializable, Request:JsonSerializable>(relativeUrl:String, request:Request?) -> Promise<Response> {
        return sendAsync(Response(), request: self.createRequest(resolveUrl(relativeUrl), httpMethod:HttpMethods.Post, request:request))
    }


    public func put<T : IReturn where T : JsonSerializable>(request:T) throws -> T.Return {
        return try send(T.Return(), request: self.createRequest(replyUrl.combinePath(T.typeName), httpMethod:HttpMethods.Put, request:request))
    }

    public func put<T : IReturnVoid where T : JsonSerializable>(request:T) throws -> Void {
        try send(ReturnVoid.void, request: self.createRequest(replyUrl.combinePath(T.typeName), httpMethod:HttpMethods.Put, request:request))
    }

    public func put<Response : JsonSerializable, Request:JsonSerializable>(relativeUrl:String, request:Request?) throws -> Response {
        return try send(Response(), request: self.createRequest(resolveUrl(relativeUrl), httpMethod:HttpMethods.Put, request:request))
    }

    public func putAsync<T : IReturn where T : JsonSerializable>(request:T) -> Promise<T.Return> {
        return sendAsync(T.Return(), request: self.createRequest(replyUrl.combinePath(T.typeName), httpMethod:HttpMethods.Put, request:request))
    }

    public func putAsync<T : IReturnVoid where T : JsonSerializable>(request:T) -> Promise<Void> {
        return sendAsync(ReturnVoid.void, request: self.createRequest(replyUrl.combinePath(T.typeName), httpMethod:HttpMethods.Put, request:request))
            .then({ x -> Void in })
    }

    public func putAsync<Response : JsonSerializable, Request:JsonSerializable>(relativeUrl:String, request:Request?) -> Promise<Response> {
        return sendAsync(Response(), request: self.createRequest(resolveUrl(relativeUrl), httpMethod:HttpMethods.Put, request:request))
    }


    public func delete<T : IReturn where T : JsonSerializable>(request:T) throws -> T.Return {
        return try send(T.Return(), request: self.createRequest(self.createUrl(request), httpMethod:HttpMethods.Delete))
    }

    public func delete<T : IReturnVoid where T : JsonSerializable>(request:T) throws -> Void {
        try send(ReturnVoid.void, request: self.createRequest(self.createUrl(request), httpMethod:HttpMethods.Delete))
    }

    public func delete<T : IReturn where T : JsonSerializable>(request:T, query:[String:String]) throws -> T.Return {
        return try send(T.Return(), request: self.createRequest(self.createUrl(request, query:query), httpMethod:HttpMethods.Delete))
    }

    public func delete<T : JsonSerializable>(relativeUrl:String) throws -> T {
        return try send(T(), request: self.createRequest(resolveUrl(relativeUrl), httpMethod:HttpMethods.Delete))
    }

    public func deleteAsync<T : IReturn where T : JsonSerializable>(request:T) -> Promise<T.Return> {
        return sendAsync(T.Return(), request: self.createRequest(self.createUrl(request), httpMethod:HttpMethods.Delete))
    }

    public func deleteAsync<T : IReturnVoid where T : JsonSerializable>(request:T) -> Promise<Void> {
        return sendAsync(ReturnVoid.void, request: self.createRequest(self.createUrl(request), httpMethod:HttpMethods.Delete))
            .then({ x -> Void in })
    }

    public func deleteAsync<T : IReturn where T : JsonSerializable>(request:T, query:[String:String]) -> Promise<T.Return> {
        return sendAsync(T.Return(), request: self.createRequest(self.createUrl(request, query:query), httpMethod:HttpMethods.Delete))
    }

    public func deleteAsync<T : JsonSerializable>(relativeUrl:String) -> Promise<T> {
        return sendAsync(T(), request: self.createRequest(resolveUrl(relativeUrl), httpMethod:HttpMethods.Delete))
    }


    public func patch<T : IReturn where T : JsonSerializable>(request:T) throws -> T.Return {
        return try send(T.Return(), request: self.createRequest(replyUrl.combinePath(T.typeName), httpMethod:HttpMethods.Patch, request:request))
    }

    public func patch<T : IReturnVoid where T : JsonSerializable>(request:T) throws -> Void {
        try send(ReturnVoid.void, request: self.createRequest(replyUrl.combinePath(T.typeName), httpMethod:HttpMethods.Patch, request:request))
    }

    public func patch<Response : JsonSerializable, Request:JsonSerializable>(relativeUrl:String, request:Request?) throws -> Response {
        return try send(Response(), request: self.createRequest(resolveUrl(relativeUrl), httpMethod:HttpMethods.Patch, request:request))
    }

    public func patchAsync<T : IReturn where T : JsonSerializable>(request:T) -> Promise<T.Return> {
        return sendAsync(T.Return(), request: self.createRequest(replyUrl.combinePath(T.typeName), httpMethod:HttpMethods.Patch, request:request))
    }

    public func patchAsync<T : IReturnVoid where T : JsonSerializable>(request:T) -> Promise<Void> {
        return sendAsync(ReturnVoid.void, request: self.createRequest(replyUrl.combinePath(T.typeName), httpMethod:HttpMethods.Patch, request:request))
            .then({ x -> Void in })
    }

    public func patchAsync<Response : JsonSerializable, Request:JsonSerializable>(relativeUrl:String, request:Request?) -> Promise<Response> {
        return sendAsync(Response(), request: self.createRequest(resolveUrl(relativeUrl), httpMethod:HttpMethods.Patch, request:request))
    }


    public func getData(url:String) throws -> NSData {
        var error: NSError! = NSError(domain: "Migrator", code: 0, userInfo: nil)
        var response:URLResponse? = nil
        do {
            let data = try NSURLConnection.sendSynchronousRequest(NSURLRequest(URL: NSURL(string:resolveUrl(url))!), returningResponse: &response)
            return data
        } catch let error1 as NSError {
            error = error1
        }
        throw error
    }

    public func getDataAsync(url:String) -> Promise<NSData> {
        return Promise<NSData> { (complete, reject) in
            let task = self.createSession().dataTaskWithURL(NSURL(string: self.resolveUrl(url))!) { (data, response, error) in
                if error != nil {
                    reject(self.handleError(error!))
                }
                complete(data!)
            }

            task.resume()
            self.lastTask = task
        }
    }
}


extension NSHTTPURLResponse {

    //Unfortunately no API gives us the real statusDescription so using Status Code descriptions instead
    public var statusDescription:String {
        switch self.statusCode {
        case 200: return "OK"
        case 201: return "Created"
        case 202: return "Accepted"
        case 205: return "No Content"
        case 206: return "Partial Content"

        case 400: return "Bad Request"
        case 401: return "Unauthorized"
        case 403: return "Forbidden"
        case 404: return "Not Found"
        case 405: return "Method Not Allowed"
        case 406: return "Not Acceptable"
        case 407: return "Proxy Authentication Required"
        case 408: return "Request Timeout"
        case 409: return "Conflict"
        case 410: return "Gone"
        case 418: return "I'm a teapot"

        case 500: return "Internal Server Error"
        case 501: return "Not Implemented"
        case 502: return "Bad Gateway"
        case 503: return "Service Unavailable"

        default: return "\(self.statusCode)"
        }
    }
}

public struct HttpMethods
{
    static let Get = "GET"
    static let Post = "POST"
    static let Put = "PUT"
    static let Delete = "DELETE"
    static let Head = "HEAD"
    static let Options = "OPTIONS"
    static let Patch = "PATCH"
}



public class JObject
{
    var sb : String

    init(string : String? = nil) {
        sb = string ?? String()
    }

    func append(name: String, json: String?) {
        if sb.length > 0 {
            sb += ","
        }
        if let s = json {
            sb += "\"\(name)\":\(s)"
        }
        else {
            sb += "\"\(name)\":null"
        }
    }

    func toJson() -> String {
        return "{\(sb)}"
    }

    class func toJson<K : Hashable, V : JsonSerializable where K : StringSerializable>(map:[K:V]) -> String? {
        let jb = JObject()

        for (k,v) in map {
            jb.append(k.toString(), json: v.toJson())
        }

        return jb.toJson()
    }
}

public class JArray
{
    var sb : String

    init(string : String? = nil) {
        sb = string ?? String()
    }

    func append(json:String?) {
        if sb.characters.count > 0 {
            sb += ","
        }
        sb += json != nil ? "\(json!)" : "null"
    }

    func toJson() -> String {
        return "[\(sb)]"
    }
}

public class JValue
{
    static func unwrap(any:Any) -> Any {

        let mi = Mirror(reflecting: any)
        if mi.displayStyle != .Optional {
            return any
        }

        if mi.children.count == 0 { return NSNull() }
        let (_, some) = mi.children.first!
        return some
    }
}

func parseJson(_ json:String) -> AnyObject? {
    do {
        return try parseJsonThrows(json)
    } catch _ {
        return nil
    }
}

func parseJsonThrows(_ json:String) throws -> AnyObject {
    let data = json.data(using: String.Encoding.utf8)!
    return try parseJsonBytesThrows(data as NSData)
}

func parseJsonBytes(_ bytes:NSData) -> AnyObject? {
    do {
        return try parseJsonBytesThrows(bytes)
    } catch _ {
        return nil
    }
}

func parseJsonBytesThrows(_ bytes:NSData) throws -> AnyObject {
    var error: NSError! = NSError(domain: "Migrator", code: 0, userInfo: nil)
    let parsedObject: AnyObject?
    do {
        parsedObject = try JSONSerialization.JSONObjectWithData(bytes,
                options: NSJSONReadingOptions.AllowFragments)
    } catch let error1 as NSError {
        error = error1
        parsedObject = nil
    }
    if let value = parsedObject {
        return value
    }
    throw error
}

extension NSString : JsonSerializable
{
    public static var typeName:String { return "NSString" }

    public static var metadata:Metadata = Metadata.create([])

    public func toString() -> String {
        return self as String
    }

    public func toJson() -> String {
        return jsonString(self as String)
    }

    public static func fromJson(json:String) -> NSString? {
        return parseJson(json) as? NSString
    }

    public static func fromString(string: String) -> NSString? {
        return string
    }

    public static func fromObject(any:AnyObject) -> NSString?
    {
        switch any {
        case let s as NSString: return s
        default:return nil
        }
    }
}

public class ReturnVoid {
    public required init(){}
}

extension ReturnVoid : JsonSerializable
{
    public static let void = ReturnVoid()

    public static var typeName:String { return "ReturnVoid" }

    public static var metadata:Metadata = Metadata.create([])

    public func toString() -> String {
        return ""
    }

    public func toJson() -> String {
        return ""
    }

    public static func fromJson(json:String) -> NSString? {
        return nil
    }

    public static func fromString(string: String) -> NSString? {
        return nil
    }

    public static func fromObject(any:AnyObject) -> NSString?
    {
        return nil
    }
}

extension String : StringSerializable
{
    public static var typeName:String { return "String" }

    public func toString() -> String {
        return self
    }

    public func toJson() -> String {
        return jsonString(self)
    }

    public static func fromString(string: String) -> String? {
        return string
    }

    public static func fromObject(any:AnyObject) -> String?
    {
        switch any {
        case let s as String: return s
        default:return nil
        }
    }
}

extension String : JsonSerializable
{
    public static var metadata:Metadata = Metadata.create([])

    public static func fromJson(json:String) -> String? {
        return parseJson(json) as? String
    }
}

extension Character : StringSerializable
{
    public static var typeName:String { return "Character" }

    public func toString() -> String {
        return "\(self)"
    }

    public func toJson() -> String {
        return jsonString(toString())
    }

    public static func fromString(string: String) -> Character? {
        return string.length > 0 ? string[0] : nil
    }

    public static func fromObject(any:AnyObject) -> Character?
    {
        switch any {
        case let s as String: return fromString(s)
        default:return nil
        }
    }
}

extension NSDate : StringSerializable
{
    public class var typeName:String { return "NSDate" }

    public func toString() -> String {
        return self.dateAndTimeString
    }

    public func toJson() -> String {
        return jsonString(self.jsonDate)
    }

    public class func fromString(string: String) -> NSDate? {
        let str = string.hasPrefix("\\")
            ? string[1..<string.length]
            : string
        let wcfJsonPrefix = "/Date("
        if str.hasPrefix(wcfJsonPrefix) {
            let body = str.splitOnFirst("(")[1].splitOnLast(")")[0]
            let unixTime = (
                body
                    .splitOnFirst("-", startIndex:1)[0]
                    .splitOnFirst("+", startIndex:1)[0] as NSString
            ).doubleValue
            return NSDate(timeIntervalSince1970: unixTime / 1000) //ms -> secs
        }

        return NSDate.fromIsoDateString(string)
    }

    public class func fromObject(any:AnyObject) -> NSDate?
    {
        switch any {
        case let s as String: return fromString(s)
        case let d as NSDate: return d
        default:return nil
        }
    }
}

extension Double : StringSerializable
{
    public static var typeName:String { return "Double" }

    public func toString() -> String {
        return "\(self)"
    }

    public func toJson() -> String {
        return "\(self)"
    }

    public static func fromString(str: String) -> Double? {
        return str.hasPrefix("P")
            ? NSTimeInterval.fromTimeIntervalString(str)
            : (str as NSString).doubleValue
    }

    public static func fromObject(any:AnyObject) -> Double?
    {
        switch any {
        case let d as Double: return d
        case let i as Int: return Double(i)
        case let s as String: return fromString(s)
        default:return nil
        }
    }
}

extension NSTimeInterval
{
    public static let ticksPerSecond:Double = 10000000;

    public func toXsdDuration() -> String {
        var sb = "P"

        let totalSeconds:Double = self
        let wholeSeconds = Int(totalSeconds)
        var seconds = wholeSeconds
        let sec = (seconds >= 60 ? seconds % 60 : seconds)
        seconds = (seconds / 60)
        let min = seconds >= 60 ? seconds % 60 : seconds
        seconds = (seconds / 60)
        let hours = seconds >= 24 ? seconds % 24 : seconds
        let days = seconds / 24
        let remainingSecs:Double = Double(sec) + (totalSeconds - Double(wholeSeconds))

        if days > 0 {
            sb += "\(days)D"
        }

        if days == 0 || Double(hours + min + sec) + remainingSecs > 0 {

            sb += "T"
            if hours > 0 {
                sb += "\(hours)H";
            }
            if min > 0 {
                sb += "\(min)M";
            }

            if remainingSecs > 0 {
                var secFmt = String(format:"%.7f", remainingSecs)
                secFmt = secFmt.trimEnd("0").trimEnd(".")
                sb += "\(secFmt)S"
            }
            else if sb.length == 2 { //PT
                sb += "0S"
            }
        }

        return sb
    }

    public func toTimeIntervalJson() -> String {
        return jsonString(toString())
    }

    public static func fromXsdDuration(xsdString:String) -> NSTimeInterval?  {
        return NSTimeInterval.fromTimeIntervalString(xsdString)
    }

    public static func fromTimeIntervalString(string:String) -> NSTimeInterval? {
        var days = 0
        var hours = 0
        var minutes = 0
        var seconds = 0
        var ms = 0.0

        let t = string[1..<string.length].splitOnFirst("T") //strip P

        let hasTime = t.count == 2

        let d = t[0].splitOnFirst("D")
        if d.count == 2 {
            if let day = Int(d[0]) {
                days = day
            }
        }

        if hasTime {
            let h = t[1].splitOnFirst("H")
            if h.count == 2 {
                if let hour = Int(h[0]) {
                    hours = hour
                }
            }

            let m = h.last!.splitOnFirst("M")
            if m.count == 2 {
                if let min = Int(m[0]) {
                    minutes = min
                }
            }

            let s = m.last!.splitOnFirst("S")
            if s.count == 2 {
                ms = s[0].toDouble()
            }

            seconds = Int(ms)
            ms -= Double(seconds)
        }

        let totalSecs = 0
            + (days * 24 * 60 * 60)
            + (hours * 60 * 60)
            + (minutes * 60)
            + (seconds)

        let interval = Double(totalSecs) + ms

        return interval
    }

    public static func fromTimeIntervalObject(any:AnyObject) -> NSTimeInterval?
    {
        switch any {
        case let s as String: return fromTimeIntervalString(s)
        case let t as NSTimeInterval: return t
        default:return nil
        }
    }
}

extension Int : StringSerializable
{
    public static var typeName:String { return "Int" }

    public func toString() -> String {
        return "\(self)"
    }

    public func toJson() -> String {
        return "\(self)"
    }

    public static func fromString(str: String) -> Int? {
        return Int(str)
    }

    public static func fromObject(any:AnyObject) -> Int?
    {
        switch any {
        case let i as Int: return i
        case let s as String: return fromString(s)
        default:return nil
        }
    }
}

extension Int8 : StringSerializable
{
    public static var typeName:String { return "Int8" }

    public func toString() -> String {
        return "\(self)"
    }

    public func toJson() -> String {
        return "\(self)"
    }

    public static func fromString(str: String) -> Int8? {
        if let int = Int(str) {
            return Int8(int)
        }
        return nil
    }

    public static func fromObject(any:AnyObject) -> Int8?
    {
        switch any {
        case let i as Int: return Int8(i)
        case let s as String: return fromString(s)
        default:return nil
        }
    }
}

extension Int16 : StringSerializable
{
    public static var typeName:String { return "Int16" }

    public func toString() -> String {
        return "\(self)"
    }

    public func toJson() -> String {
        return "\(self)"
    }

    public static func fromString(str: String) -> Int16? {
        if let int = Int(str) {
            return Int16(int)
        }
        return nil
    }

    public static func fromObject(any:AnyObject) -> Int16?
    {
        switch any {
        case let i as Int: return Int16(i)
        case let s as String: return fromString(s)
        default:return nil
        }
    }
}

extension Int32 : StringSerializable
{
    public static var typeName:String { return "Int32" }

    public func toString() -> String {
        return "\(self)"
    }

    public func toJson() -> String {
        return "\(self)"
    }

    public static func fromString(str: String) -> Int32? {
        if let int = Int(str) {
            return Int32(int)
        }
        return nil
    }

    public static func fromObject(any:AnyObject) -> Int32?
    {
        switch any {
        case let i as Int: return Int32(i)
        case let s as String: return fromString(s)
        default:return nil
        }
    }
}

extension Int64 : StringSerializable
{
    public static var typeName:String { return "Int64" }

    public func toString() -> String {
        return "\(self)"
    }

    public func toJson() -> String {
        return "\(self)"
    }

    public static func fromString(str: String) -> Int64? {
        return (str as NSString).longLongValue
    }

    public static func fromObject(any:AnyObject) -> Int64?
    {
        switch any {
        case let i as Int: return Int64(i)
        case let s as String: return fromString(s)
        default:return nil
        }
    }
}

extension UInt8 : StringSerializable
{
    public static var typeName:String { return "UInt8" }

    public func toString() -> String {
        return "\(self)"
    }

    public func toJson() -> String {
        return "\(self)"
    }

    public static func fromString(str: String) -> UInt8? {
        if let int = Int(str) {
            return UInt8(int)
        }
        return nil
    }

    public static func fromObject(any:AnyObject) -> UInt8?
    {
        switch any {
        case let i as Int: return UInt8(i)
        case let s as String: return fromString(s)
        default:return nil
        }
    }
}

extension UInt16 : StringSerializable
{
    public static var typeName:String { return "UInt16" }

    public func toString() -> String {
        return "\(self)"
    }

    public func toJson() -> String {
        return "\(self)"
    }

    public static func fromString(str: String) -> UInt16? {
        if let int = Int(str) {
            return UInt16(int)
        }
        return nil
    }

    public static func fromObject(any:AnyObject) -> UInt16?
    {
        switch any {
        case let i as Int: return UInt16(i)
        case let s as String: return fromString(s)
        default:return nil
        }
    }
}

extension UInt32 : StringSerializable
{
    public static var typeName:String { return "UInt32" }

    public func toString() -> String {
        return "\(self)"
    }

    public func toJson() -> String {
        return "\(self)"
    }

    public static func fromString(str: String) -> UInt32? {
        if let int = Int(str) {
            return UInt32(int)
        }
        return nil
    }

    public static func fromObject(any:AnyObject) -> UInt32?
    {
        switch any {
        case let i as Int: return UInt32(i)
        case let s as String: return fromString(s)
        default:return nil
        }
    }
}

extension UInt64 : StringSerializable
{
    public static var typeName:String { return "UInt64" }

    public func toString() -> String {
        return "\(self)"
    }

    public func toJson() -> String {
        return "\(self)"
    }

    public static func fromString(str: String) -> UInt64? {
        return UInt64((str as NSString).longLongValue)
    }

    public static func fromObject(any:AnyObject) -> UInt64?
    {
        switch any {
        case let i as Int: return UInt64(i)
        case let s as String: return fromString(s)
        default:return nil
        }
    }
}

extension Float : StringSerializable
{
    public static var typeName:String { return "Float" }

    public func toString() -> String {
        return "\(self)"
    }

    public func toJson() -> String {
        return "\(self)"
    }

    public static func fromString(str: String) -> Float? {
        return (str as NSString).floatValue
    }

    public static func fromObject(any:AnyObject) -> Float?
    {
        switch any {
        case let f as Float: return f
        case let i as Int: return Float(i)
        case let s as String: return fromString(s)
        default:return nil
        }
    }
}

extension Bool : StringSerializable
{
    public static var typeName:String { return "Bool" }

    public func toString() -> String {
        return "\(self)"
    }

    public func toJson() -> String {
        return "\(self)"
    }

    public static func fromString(str: String) -> Bool? {
        return str.lowercaseString == "true"
    }

    public static func fromObject(any:AnyObject) -> Bool?
    {
        switch any {
        case let b as Bool: return b
        case let i as Int: return i != 0
        case let s as String: return fromString(s)
        default:return nil
        }
    }
}

public class ResponseStatus
{
    required public init(){}
    public var errorCode:String?
    public var message:String?
    public var stackTrace:String?
    public var errors:[ResponseError] = []
    public var meta:[String:String] = [:]
}

extension ResponseStatus : JsonSerializable
{
    public static var typeName:String { return "ResponseStatus" }
    public static var metadata = Metadata.create([
        Type<ResponseStatus>.optionalProperty("errorCode", get: { $0.errorCode }, set: { $0.errorCode = $1 }),
        Type<ResponseStatus>.optionalProperty("message", get: { $0.message }, set: { $0.message = $1 }),
        Type<ResponseStatus>.optionalProperty("stackTrace", get: { $0.stackTrace }, set: { $0.stackTrace = $1 }),
        Type<ResponseStatus>.arrayProperty("errors", get: { $0.errors }, set: { $0.errors = $1 }),
        Type<ResponseStatus>.objectProperty("meta", get: { $0.meta }, set: { $0.meta = $1 }),
        ])
}

public class ResponseError
{
    required public init(){}
    public var errorCode:String?
    public var fieldName:String?
    public var message:String?
    public var meta:[String:String] = [:]
}

extension ResponseError : JsonSerializable
{
    public static var typeName:String { return "ResponseError" }
    public static var metadata = Metadata.create([
        Type<ResponseError>.optionalProperty("errorCode", get: { $0.errorCode }, set: { $0.errorCode = $1 }),
        Type<ResponseError>.optionalProperty("fieldName", get: { $0.fieldName }, set: { $0.fieldName = $1 }),
        Type<ResponseError>.optionalProperty("message", get: { $0.message }, set: { $0.message = $1 }),
        Type<ResponseError>.objectProperty("meta", get: { $0.meta }, set: { $0.meta = $1 }),
        ])
}

public class ErrorResponse
{
    required public init(){}
    public var responseStatus:ResponseStatus?
}

extension ErrorResponse : JsonSerializable
{
    public static var typeName:String { return "ResponseError" }
    public static var metadata = Metadata.create([
        Type<ErrorResponse>.optionalObjectProperty("responseStatus", get: { $0.responseStatus }, set: { $0.responseStatus = $1 }),
        ])
}



public class List<T>
{
    required public init(){}
}

public protocol HasMetadata {
    static var typeName:String { get }
    static var metadata:Metadata { get }
    init()
}

public protocol Convertible {
    typealias T
    static var typeName:String { get }
    static func fromObject(any:AnyObject) -> T?
}

public protocol JsonSerializable : HasMetadata, StringSerializable {
    func toJson() -> String
    static func fromJson(json:String) -> T?
}

public protocol StringSerializable : Convertible {
    func toJson() -> String
    func toString() -> String
    static func fromString(string:String) -> T?
}


public func populate<T : HasMetadata>(instance:T, map:NSDictionary, propertiesMap:[String:PropertyType]) -> T {
    for (key, obj) in map {
        if let p = propertiesMap[key.lowercaseString] {
            p.setValueAny(instance as! AnyObject, value: obj)
        }
    }
    return instance
}

public func populateFromDictionary<T : JsonSerializable>(instance:T, map:[NSObject : AnyObject], propertiesMap:[String:PropertyType]) -> T {
    for (key, obj) in map {
        if let strKey = key as? String {
            if let p = propertiesMap[strKey.lowercaseString] {
                p.setValueAny(instance as! AnyObject, value: obj)
            }
        }
    }
    return instance
}

public class Metadata {
    public var properties:[PropertyType] = []
    public var propertyMap:[String:PropertyType] = [:]

    public init(properties:[PropertyType]) {
        self.properties = properties
        for p in properties {
            propertyMap[p.name.lowercaseString] = p
        }
    }

    static func create(properties:[PropertyType]) -> Metadata {
        return Metadata(properties: properties)
    }
}

extension HasMetadata
{
    public static var properties:[PropertyType] {
        return Self.metadata.properties
    }

    public static var propertyMap:[String:PropertyType] {
        return Self.metadata.propertyMap
    }

    public func toJson() -> String {
        let jb = JObject()
        for p in Self.properties {
            if let value = p.jsonValueAny(self) {
                jb.append(p.name, json: value)
            } else {
                jb.append(p.name, json: "null")
            }
        }
        return jb.toJson()
    }

    public static func fromJson(json:String) -> Self? {
        if let map = parseJson(json) as? NSDictionary {
            return populate(Self(), map: map, propertiesMap: Self.propertyMap)
        }
        return nil
    }

    public static func fromObject(any:AnyObject) -> Self? {
        switch any {
        case let s as String: return fromJson(s)
        case let map as NSDictionary: return populate(Self(), map: map, propertiesMap: Self.propertyMap)
        default: return nil
        }
    }

    public func toString() -> String {
        return toJson()
    }

    public static func fromString(string:String) -> Self? {
        return fromJson(string)
    }
}

public class TypeAccessor {}

public class Type<T : HasMetadata> : TypeAccessor
{
    var properties: [PropertyType]
    var propertiesMap = [String:PropertyType]()

    init(properties:[PropertyType])
    {
        self.properties = properties

        for p in properties {
            propertiesMap[p.name.lowercaseString] = p
        }
    }

    static public func toJson(instance:T) -> String {
        let jb = JObject()
        for p in T.properties {
            if let value = p.jsonValueAny(instance) {
                jb.append(p.name, json: value)
            } else {
                jb.append(p.name, json: "null")
            }
        }
        return jb.toJson()
    }

    static public func toString(instance:T) -> String {
        return toJson(instance)
    }

    static func fromJson<T : JsonSerializable>(json:String) -> T? {
        return fromJson(T(), json: json)
    }

    static func fromString<T : JsonSerializable>(instance:T, string:String) -> T? {
        return fromJson(instance, json: string)
    }

    static func fromObject<T : JsonSerializable>(instance:T, any:AnyObject) -> T? {
        switch any {
        case let s as String: return fromJson(instance, json: s)
        case let map as NSDictionary: return Type<T>.fromDictionary(instance, map: map)
        default: return nil
        }
    }

    static func fromJson<T : JsonSerializable>(instance:T, json:String) -> T? {
        if instance is NSString || instance is String {
            if let value = json as? T {
                return value
            }
        }
        if let map = parseJson(json) as? NSDictionary {
            return populate(instance, map: map, propertiesMap: T.propertyMap)
        }
        return nil
    }

    static func fromDictionary<T : HasMetadata>(instance:T, map:NSDictionary) -> T {
        return populate(instance, map: map, propertiesMap: T.propertyMap)
    }

    public class func property<P : StringSerializable>(name:String, get:(T) -> P, set:(T,P) -> Void) -> PropertyType
    {
        return JProperty(name: name, get:get, set:set)
    }

    public class func optionalProperty<P : StringSerializable>(name:String, get:(T) -> P?, set:(T,P?) -> Void) -> PropertyType
    {
        return JOptionalProperty(name: name, get:get, set:set)
    }

    public class func objectProperty<P : JsonSerializable>(name:String, get:(T) -> P, set:(T,P) -> Void) -> PropertyType
    {
        return JObjectProperty(name: name, get:get, set:set)
    }

    public class func optionalObjectProperty<P : JsonSerializable>(name:String, get:(T) -> P?, set:(T,P?) -> Void) -> PropertyType
    {
        return JOptionalObjectProperty(name: name, get:get, set:set)
    }

    public class func objectProperty<K : Hashable, P : StringSerializable where K : StringSerializable>(name:String, get:(T) -> [K:P], set:(T,[K:P]) -> Void) -> PropertyType
    {
        return JDictionaryProperty(name: name, get:get, set:set)
    }

    public class func objectProperty<K : Hashable, P : StringSerializable where K : StringSerializable, K == K.T>(name:String, get:(T) -> [K:[P]], set:(T,[K:[P]]) -> Void) -> PropertyType
    {
        return JDictionaryArrayProperty(name: name, get:get, set:set)
    }

    public class func objectProperty<K : Hashable, P : JsonSerializable where K : StringSerializable>(name:String, get:(T) -> [K:[K:P]], set:(T,[K:[K:P]]) -> Void) -> PropertyType
    {
        return JDictionaryArrayDictionaryObjectProperty(name: name, get:get, set:set)
    }

    public class func arrayProperty<P : StringSerializable>(name:String, get:(T) -> [P], set:(T,[P]) -> Void) -> PropertyType
    {
        return JArrayProperty(name: name, get:get, set:set)
    }

    public class func optionalArrayProperty<P : StringSerializable>(name:String, get:(T) -> [P]?, set:(T,[P]?) -> Void) -> PropertyType
    {
        return JOptionalArrayProperty(name: name, get:get, set:set)
    }

    public class func arrayProperty<P : JsonSerializable>(name:String, get:(T) -> [P], set:(T,[P]) -> Void) -> PropertyType
    {
        return JArrayObjectProperty(name: name, get:get, set:set)
    }

    public class func optionalArrayProperty<P : JsonSerializable>(name:String, get:(T) -> [P]?, set:(T,[P]?) -> Void) -> PropertyType
    {
        return JOptionalArrayObjectProperty(name: name, get:get, set:set)
    }
}

public class PropertyType {
    public var name:String

    init(name:String) {
        self.name = name
    }

    public func jsonValueAny(instance:Any) -> String? {
        return nil
    }

    public func setValueAny(instance:Any, value:AnyObject) {
    }

    public func getValueAny(instance:Any) -> Any? {
        return nil
    }

    public func stringValueAny(instance:Any) -> String? {
        return nil
    }

    public func getName() -> String {
        return self.name
    }
}

public class PropertyBase<T : HasMetadata> : PropertyType {

    override init(name:String) {
        super.init(name: name)
    }

    public override func jsonValueAny(instance:Any) -> String? {
        return jsonValue(instance as! T)
    }

    public func jsonValue(instance:T) -> String? {
        return nil
    }

    public override func setValueAny(instance:Any, value:AnyObject) {
        if let t = instance as? T {
            setValue(t, value: value)
        }
    }

    public func setValue(instance:T, value:AnyObject) {
    }

    public override func getValueAny(instance:Any) -> Any? {
        return getValue(instance as! T)
    }

    public func getValue(instance:T) -> Any? {
        return nil
    }

    public override func stringValueAny(instance:Any) -> String? {
        return stringValue(instance as! T)
    }

    public func stringValue(instance:T) -> String? {
        return nil
    }
}

public class JProperty<T : HasMetadata, P : StringSerializable> : PropertyBase<T>
{
    public var get:(T) -> P
    public var set:(T,P) -> Void

    init(name:String, get:(T) -> P, set:(T,P) -> Void)
    {
        self.get = get
        self.set = set
        super.init(name: name)
    }

    public override func getValue(instance: T) -> Any? {
        return get(instance) as Any
    }

    public override func stringValue(instance:T) -> String? {
        return get(instance).toString()
    }

    public override func jsonValue(instance:T) -> String? {
        return get(instance).toJson()
    }

    public override func setValue(instance:T, value:AnyObject) {
        if let p = value as? P {
            set(instance, p)
        }
        else if let p = P.fromObject(value) as? P {
            set(instance, p)
        }
    }
}

public class JOptionalProperty<T : HasMetadata, P : StringSerializable> : PropertyBase<T>
{
    public var get:(T) -> P?
    public var set:(T,P?) -> Void

    init(name:String, get:(T) -> P?, set:(T,P?) -> Void)
    {
        self.get = get
        self.set = set
        super.init(name: name)
    }

    public override func stringValue(instance:T) -> String? {
        if let p = get(instance) {
            return p.toString()
        }
        return super.jsonValue(instance)
    }

    public override func jsonValue(instance:T) -> String? {
        if let p = get(instance) {
            return p.toJson()
        }
        return super.jsonValue(instance)
    }

    public override func getValue(instance:T) -> Any? {
        if let p = get(instance) {
            return p
        }
        return nil
    }

    public override func setValue(instance:T, value:AnyObject) {
        if let p = value as? P {
            set(instance, p)
        }
        else if let p = P.fromObject(value) as? P {
            set(instance, p)
        }
    }
}


public class JObjectProperty<T : HasMetadata, P : JsonSerializable> : PropertyBase<T>
{
    public var get:(T) -> P
    public var set:(T,P) -> Void

    init(name:String, get:(T) -> P, set:(T,P) -> Void)
    {
        self.get = get
        self.set = set
        super.init(name: name)
    }

    public override func getValue(instance: T) -> Any? {
        return get(instance) as Any
    }

    public override func stringValue(instance:T) -> String? {
        return get(instance).toString()
    }

    public override func jsonValue(instance:T) -> String? {
        return get(instance).toJson()
    }

    public override func setValue(instance:T, value:AnyObject) {
        if let p = value as? P {
            set(instance, p)
        }
        else if let p = P.fromObject(value) as? P {
            set(instance, p)
        }
    }
}

public class JOptionalObjectProperty<T : HasMetadata, P : JsonSerializable where P : HasMetadata> : PropertyBase<T>
{
    public var get:(T) -> P?
    public var set:(T,P?) -> Void

    init(name:String, get:(T) -> P?, set:(T,P?) -> Void)
    {
        self.get = get
        self.set = set
        super.init(name: name)
    }

    public override func jsonValue(instance:T) -> String? {
        if let propValue = get(instance) {
            let strValue = propValue.toJson()
            return strValue
        }
        return super.jsonValue(instance)
    }

    public override func setValue(instance:T, value:AnyObject) {
        if let map = value as? NSDictionary {
            let p = Type<P>.fromDictionary(P(), map: map)
            set(instance, p)
        }
    }
}

public class JDictionaryProperty<T : HasMetadata, K : Hashable, P : StringSerializable where K : StringSerializable> : PropertyBase<T>
{
    public var get:(T) -> [K:P]
    public var set:(T,[K:P]) -> Void

    init(name:String, get:(T) -> [K:P], set:(T,[K:P]) -> Void)
    {
        self.get = get
        self.set = set
        super.init(name: name)
    }

    public override func getValue(instance: T) -> Any? {
        return get(instance) as Any
    }

    public override func stringValue(instance:T) -> String? {
        return jsonValue(instance)
    }

    public override func jsonValue(instance:T) -> String? {
        let map = get(instance)

        let jb = JObject()
        for (key, value) in map {
            jb.append(key.toString(), json:value.toJson())
        }
        return jb.toJson()
    }

    public override func setValue(instance:T, value:AnyObject) {
        if let map = value as? NSDictionary {
            var to = [K:P]()
            for (key, obj) in map {
                if let keyK = K.fromObject(key) as? K {
                    if let valueP = P.fromObject(obj) as? P {
                        to[keyK] = valueP
                    }
                }
            }
            set(instance, to)
        }
    }
}

public class JDictionaryArrayProperty<T : HasMetadata, K : Hashable, P : StringSerializable where K : StringSerializable, K == K.T> : PropertyBase<T>
{
    public var get:(T) -> [K:[P]]
    public var set:(T,[K:[P]]) -> Void

    init(name:String, get:(T) -> [K:[P]], set:(T,[K:[P]]) -> Void)
    {
        self.get = get
        self.set = set
        super.init(name: name)
    }

    public override func getValue(instance: T) -> Any? {
        return get(instance) as Any
    }

    public override func stringValue(instance:T) -> String? {
        return jsonValue(instance)
    }

    public override func jsonValue(instance:T) -> String? {
        let map = get(instance)

        let jb = JObject()
        for (key, values) in map {
            let ja = JArray()
            for v in values {
                ja.append(v.toJson())
            }
            jb.append(key.toString(), json:ja.toJson())
        }
        return jb.toJson()
    }

    public override func setValue(instance:T, value:AnyObject) {
        if let map = value as? NSDictionary {
            var to = [K:[P]]()
            for (key, obj) in map {
                if let keyK = K.fromObject(key) {
                    var valuesP = to[keyK] ?? [P]()
                    if let valuesArray = obj as? NSArray {
                        for item in valuesArray {
                            if let valueP = P.fromObject(item) as? P {
                                valuesP.append(valueP)
                            }
                        }
                    }
                    to[keyK] = valuesP
                }
            }
            set(instance, to)
        }
    }
}

public class JDictionaryArrayDictionaryObjectProperty<T : HasMetadata, K : Hashable, P : JsonSerializable where K : StringSerializable> : PropertyBase<T>
{
    public var get:(T) -> [K:[K:P]]
    public var set:(T,[K:[K:P]]) -> Void

    init(name:String, get:(T) -> [K:[K:P]], set:(T,[K:[K:P]]) -> Void)
    {
        self.get = get
        self.set = set
        super.init(name: name)
    }

    public override func getValue(instance: T) -> Any? {
        return get(instance) as Any
    }

    public override func stringValue(instance:T) -> String? {
        return jsonValue(instance)
    }

    public override func jsonValue(instance:T) -> String? {
        let map = get(instance)

        let jb = JObject()
        for (key, values) in map {
            jb.append(key.toString(), json:JObject.toJson(values))
        }
        return jb.toJson()
    }

    public override func setValue(instance:T, value:AnyObject) {
        if let map = value as? NSDictionary {
            var to = [K:[K:P]]()
            for (k,vArray) in map {
                var values = [K:P]()
                if let array = vArray as? NSArray {
                    for item in array {
                        if let map = item as? NSDictionary {
                            for (subK, subV) in map {
                                values[K.fromObject(subK)! as! K] = P.fromObject(subV) as? P
                            }
                        }
                    }
                }
                to[K.fromObject(k) as! K] = values
            }
            set(instance,to)
        }
    }
}

public class JArrayProperty<T : HasMetadata, P : StringSerializable> : PropertyBase<T>
{
    public var get:(T) -> [P]
    public var set:(T,[P]) -> Void

    init(name:String, get:(T) -> [P], set:(T,[P]) -> Void)
    {
        self.get = get
        self.set = set
        super.init(name: name)
    }

    public override func getValue(instance: T) -> Any? {
        return get(instance) as Any
    }

    public override func stringValue(instance:T) -> String? {
        return jsonValue(instance)
    }

    public override func jsonValue(instance:T) -> String? {
        let propValues = get(instance)

        var sb = ""

        for item in propValues {
            if sb.length > 0 {
                sb += ","
            }
            var str:String = "null"
            str = item.toJson()

            sb += str
        }

        return "[\(sb)]"
    }

    public override func setValue(instance:T, value:AnyObject) {
        if let array = value as? NSArray {
            if array.count == 0 {
                return
            }
            var to = [P]()
            for item in array {
                if let pValue = P.fromObject(item) as? P {
                    to.append(pValue)
                }
            }
            set(instance, to)
        }
    }
}

public class JOptionalArrayProperty<T : HasMetadata, P : StringSerializable> : PropertyBase<T>
{
    public var get:(T) -> [P]?
    public var set:(T,[P]?) -> Void

    init(name:String, get:(T) -> [P]?, set:(T,[P]?) -> Void)
    {
        self.get = get
        self.set = set
        super.init(name: name)
    }

    public override func getValue(instance: T) -> Any? {
        return get(instance) as Any
    }

    public override func stringValue(instance:T) -> String? {
        return jsonValue(instance)
    }

    public override func jsonValue(instance:T) -> String? {
        var sb = ""
        if let propValues = get(instance) {
            for item in propValues {
                if sb.length > 0 {
                    sb += ","
                }
                var str:String = "null"
                str = item.toJson()

                sb += str
            }
        } else {
            return "null"
        }

        return "[\(sb)]"
    }

    public override func setValue(instance:T, value:AnyObject) {
        if let array = value as? NSArray {
            if array.count == 0 {
                return
            }
            var to = [P]()
            for item in array {
                if let pValue = P.fromObject(item) as? P {
                    to.append(pValue)
                }
            }
            set(instance, to)
        }
    }
}

public class JArrayObjectProperty<T : HasMetadata, P : JsonSerializable> : PropertyBase<T>
{
    public var get:(T) -> [P]
    public var set:(T,[P]) -> Void

    init(name:String, get:(T) -> [P], set:(T,[P]) -> Void)
    {
        self.get = get
        self.set = set
        super.init(name: name)
    }

    public override func getValue(instance: T) -> Any? {
        return get(instance) as Any
    }

    public override func stringValue(instance:T) -> String? {
        return jsonValue(instance)
    }

    public override func jsonValue(instance:T) -> String? {
        let propValues = get(instance)

        var sb = ""

        for item in propValues {
            if sb.length > 0 {
                sb += ","
            }
            var str:String = "null"
            str = item.toJson()

            sb += str
        }

        return "[\(sb)]"
    }

    public override func setValue(instance:T, value:AnyObject) {
        if let array = value as? NSArray {
            if array.count == 0 {
                return
            }
            var to = [P]()
            for item in array {
                if let pValue = P.fromObject(item) as? P {
                    to.append(pValue)
                }
            }
            set(instance, to)
        }
    }
}

public class JOptionalArrayObjectProperty<T : HasMetadata, P : JsonSerializable> : PropertyBase<T>
{
    public var get:(T) -> [P]?
    public var set:(T,[P]?) -> Void

    init(name:String, get:(T) -> [P]?, set:(T,[P]?) -> Void)
    {
        self.get = get
        self.set = set
        super.init(name: name)
    }

    public override func getValue(instance: T) -> Any? {
        return get(instance) as Any
    }

    public override func stringValue(instance:T) -> String? {
        return jsonValue(instance)
    }

    public override func jsonValue(instance:T) -> String? {
        var sb = ""

        if let propValues = get(instance) {
            for item in propValues {
                if sb.length > 0 {
                    sb += ","
                }
                var str:String = "null"
                str = item.toJson()

                sb += str
            }
        } else {
            return "null"
        }

        return "[\(sb)]"
    }

    public override func setValue(instance:T, value:AnyObject) {
        if let array = value as? NSArray {
            if array.count == 0 {
                return
            }
            var to = [P]()
            for item in array {
                if let pValue = P.fromObject(item) as? P {
                    to.append(pValue)
                }
            }
            set(instance, to)
        }
    }
}

class TypeConfig
{
    struct Config {
        static var types = Dictionary<String, TypeAccessor>()
    }

    class func configure<T : Convertible>(typeConfig:Type<T>) -> Type<T> {
        Config.types[T.typeName] = typeConfig
        return typeConfig
    }

    class func config<T : Convertible>() -> Type<T>? {
        if let typeInfo = Config.types[T.typeName] {
            return typeInfo as? Type<T>
        }
        return nil
    }
}

func jsonStringRaw(str:String?) -> String {
    if let s = str {
        return "\"\(s)\""
    }
    else {
        return "null"
    }
}

class Utils
{
    class func escapeChars() -> NSCharacterSet {
        return NSCharacterSet(charactersInString: "\"\n\r\t\\")
    }
}

func jsonString(str:String?) -> String {
    if let s = str {
        if let _ = s.rangeOfCharacterFromSet(Utils.escapeChars()) {
            do {
                let encodedData = try JSONSerialization.dataWithJSONObject([s], options:NSJSONWritingOptions())
                if let encodedJson = encodedData.toUtf8String() {
                    return encodedJson[1..<encodedJson.length-1] //strip []
                }
            } catch { }
        }
        return "\"\(s)\""
    }
    else {
        return "null"
    }
}



public extension String
{
    public var length: Int { return self.characters.count }

    public func contains(s:String) -> Bool {
        return (self as NSString).containsString(s)
    }

    public func trim() -> String {
        return (self as String).stringByTrimmingCharactersInSet(.whitespaceCharacterSet())
    }

    public func trimEnd(needle: Character) -> String {
        var i: Int = self.characters.count - 1, j: Int = i

        while i >= 0 && self[self.startIndex.advancedBy(i)] == needle {
            --i
        }

        return self.substringWithRange(Range<String.Index>(start: self.startIndex, end: self.endIndex.advancedBy(-(j - i))))
    }

    public subscript (i: Int) -> Character {
        return self[self.startIndex.advancedBy(i)]
    }

    public subscript (i: Int) -> String {
        return String(self[i] as Character)
    }

    public subscript (r: Range<Int>) -> String {
        return substringWithRange(Range(start: startIndex.advancedBy(r.startIndex), end: startIndex.advancedBy(r.endIndex)))
    }

    public func urlEncode() -> String? {
        return self.stringByAddingPercentEncodingWithAllowedCharacters(.URLHostAllowedCharacterSet())
    }

    public func combinePath(path:String) -> String {
        return (self.hasSuffix("/") ? self : self + "/") + (path.hasPrefix("/") ? path[1..<path.length] : path)
    }

    public func splitOnFirst(separator:String) -> [String] {
        return splitOnFirst(separator, startIndex: 0)
    }

    public func splitOnFirst(separator:String, startIndex:Int) -> [String] {
        var to = [String]()

        let startRange = self.startIndex.advancedBy(startIndex)
        if let range = self.rangeOfString(separator,
            options: NSStringCompareOptions.LiteralSearch,
            range: Range<String.Index>(start: startRange, end: self.endIndex))
        {
            to.append(self[self.startIndex..<range.startIndex])
            to.append(self[range.endIndex..<endIndex])
        }
        else {
            to.append(self)
        }
        return to
    }

    public func splitOnLast(separator:String) -> [String] {
        var to = [String]()
        if let range = self.rangeOfString(separator, options:NSStringCompareOptions.BackwardsSearch) {
            to.append(self[startIndex..<range.startIndex])
            to.append(self[range.endIndex..<endIndex])
        }
        else {
            to.append(self)
        }
        return to
    }

    public func split(separator:String) -> [String] {
        return self.componentsSeparatedByString(separator)
    }

    public func indexOf(needle:String) -> Int {
        if let range = self.rangeOfString(needle) {
            return startIndex.distanceTo(range.startIndex)
        }
        return -1
    }

    public func lastIndexOf(needle:String) -> Int {
        if let range = self.rangeOfString(needle, options:NSStringCompareOptions.BackwardsSearch) {
            return startIndex.distanceTo(range.startIndex)
        }
        return -1
    }

    public func replace(needle:String, withString:String) -> String {
        return self.stringByReplacingOccurrencesOfString(needle, withString: withString)
    }

    public func toDouble() -> Double {
        return (self as NSString).doubleValue
    }

    public func print() -> String {
        Swift.print(self)
        return self
    }

    public func stripQuotes() -> String {
        return self.hasPrefix("\"") && self.hasSuffix("\"")
            ? self[1..<self.length-1]
            : self
    }
}

extension Array
{
    func print() -> String {
        var sb = ""
        for item in self {
            if sb.length > 0 {
                sb += ","
            }
            sb += "\(item)"
        }
        Swift.print(sb)
        return sb
    }
}

extension NSData
{
    func toUtf8String() -> String? {
        if let str = NSString(data: self, encoding: NSUTF8StringEncoding) {
            return str as String
        }
        return nil
    }

    func print() -> String {
        return self.toUtf8String()!.print()
    }
}

extension NSError
{
    func convertUserInfo<T : JsonSerializable>() -> T? {
        return self.populateUserInfo(T())
    }

    func populateUserInfo<T : JsonSerializable>(instance:T) -> T? {
        let to = populateFromDictionary(T(), map: self.userInfo, propertiesMap: T.propertyMap)
        return to
    }
}


public extension NSDate {

    public convenience init(dateString:String, format:String="yyyy-MM-dd") {
        let fmt = NSDateFormatter()
        fmt.timeZone = NSTimeZone.defaultTimeZone()
        fmt.dateFormat = format
        let d = fmt.dateFromString(dateString)
        self.init(timeInterval:0, sinceDate:d!)
    }

    public convenience init(year:Int, month:Int, day:Int) {
        let c = NSDateComponents()
        c.year = year
        c.month = month
        c.day = day

        let gregorian = NSCalendar(identifier:NSCalendarIdentifierGregorian)
        let d = gregorian?.dateFromComponents(c)
        self.init(timeInterval:0, sinceDate:d!)
    }

    public func components() -> NSDateComponents {
        let components  = NSCalendar.currentCalendar().components(
            [NSCalendarUnit.Day, NSCalendarUnit.Month, NSCalendarUnit.Year],
            fromDate: self)

        return components
    }

    public var year:Int {
        return components().year
    }

    public var month:Int {
        return components().month
    }

    public var day:Int {
        return components().day
    }

    public var shortDateString:String {
        let fmt = NSDateFormatter()
        fmt.timeZone = NSTimeZone.defaultTimeZone()
        fmt.dateFormat = "yyyy-MM-dd"
        return fmt.stringFromDate(self)
    }

    public var dateAndTimeString:String {
        let fmt = NSDateFormatter()
        fmt.timeZone = NSTimeZone.defaultTimeZone()
        fmt.dateFormat = "yyyy-MM-dd HH:mm:ss"
        return fmt.stringFromDate(self)
    }

    public var jsonDate:String {
        let unixEpoch = Int64(self.timeIntervalSince1970 * 1000)
        return "/Date(\(unixEpoch)-0000)/"
    }

    public var isoDateString:String {
        let dateFormatter = NSDateFormatter()
        dateFormatter.locale = NSLocale(localeIdentifier: "en_US_POSIX")
        dateFormatter.timeZone = NSTimeZone(abbreviation: "UTC")
        dateFormatter.dateFormat = "yyyy-MM-dd'T'HH:mm:ss.SSS"
        return dateFormatter.stringFromDate(self).stringByAppendingString("Z")
    }

    public class func fromIsoDateString(string:String) -> NSDate? {
        let isUtc = string.hasSuffix("Z")
        let dateFormatter = NSDateFormatter()
        dateFormatter.locale = NSLocale(localeIdentifier: "en_US_POSIX")
        dateFormatter.timeZone = isUtc ? NSTimeZone(abbreviation: "UTC") : NSTimeZone.localTimeZone()
        dateFormatter.dateFormat = string.length == 19 || (isUtc && string.length == 20)
            ? "yyyy-MM-dd'T'HH:mm:ss"
            : "yyyy-MM-dd'T'HH:mm:ss.SSSSSSS"

        return isUtc
            ? dateFormatter.dateFromString(string[0..<string.length-1])
            : dateFormatter.dateFromString(string)
    }
}

public func >(lhs: NSDate, rhs: NSDate) -> Bool {
    return lhs.compare(rhs) == NSComparisonResult.OrderedDescending
}
public func >=(lhs: NSDate, rhs: NSDate) -> Bool {
    return lhs.compare(rhs) == NSComparisonResult.OrderedDescending
        || lhs == rhs
}
public func <(lhs: NSDate, rhs: NSDate) -> Bool {
    return lhs.compare(rhs) == NSComparisonResult.OrderedAscending
}
public func <=(lhs: NSDate, rhs: NSDate) -> Bool {
    return lhs.compare(rhs) == NSComparisonResult.OrderedAscending
        || lhs == rhs
}
public func ==(lhs: NSDate, rhs: NSDate) -> Bool {
    return lhs.compare(rhs) == NSComparisonResult.OrderedSame
}

public let PMKOperationQueue = NSOperationQueue()

public enum CatchPolicy {
    case AllErrors
    case AllErrorsExceptCancellation
}

/**
License: https://github.com/mxcl/PromiseKit#license
A promise represents the future value of a task.

To obtain the value of a promise we call `then`.

Promises are chainable: `then` returns a promise, you can call `then` on
that promise, which  returns a promise, you can call `then` on that
promise, et cetera.

Promises start in a pending state and *resolve* with a value to become
*fulfilled* or with an `NSError` to become rejected.

@see [PromiseKit `then` Guide](http://promisekit.org/then/)
@see [PromiseKit Chaining Guide](http://promisekit.org/chaining/)
*/
public class Promise<T> {
    let state: State

    public convenience init(@noescape resolvers: (fulfill: (T) -> Void, reject: (NSError) -> Void) -> Void) {
        self.init(sealant: { sealant in
            resolvers(fulfill: sealant.resolve, reject: sealant.resolve)
        })
    }

    public init(@noescape sealant: (Sealant<T>) -> Void) {
        var resolve: ((Resolution) -> Void)!
        state = UnsealedState(resolver: &resolve)
        sealant(Sealant(body: resolve))
    }

    public init(_ value: T) {
        state = SealedState(resolution: .Fulfilled(value))
    }

    public init(_ error: NSError) {
        unconsume(error)
        state = SealedState(resolution: .Rejected(error))
    }

    init(@noescape passthru: ((Resolution) -> Void) -> Void) {
        var resolve: ((Resolution) -> Void)!
        state = UnsealedState(resolver: &resolve)
        passthru(resolve)
    }

    public class func pendingPromise() -> (promise: Promise, fulfill: (T) -> Void, reject: (NSError) -> Void) {
        var sealant: Sealant<T>!
        let promise = Promise { sealant = $0 }
        return (promise, sealant.resolve, sealant.resolve)
    }

    func pipe(body: (Resolution) -> Void) {
        state.get { seal in
            switch seal {
            case .Pending(let handlers):
                handlers.append(body)
            case .Resolved(let resolution):
                body(resolution)
            }
        }
    }

    private convenience init<U>(when: Promise<U>, body: (Resolution, (Resolution) -> Void) -> Void) {
        self.init(passthru: { resolve in
            when.pipe{ body($0, resolve) }
        })
    }

    public func then<U>(on q: dispatch_queue_t = dispatch_get_main_queue(), _ body: (T) -> U) -> Promise<U> {
        return Promise<U>(when: self) { resolution, resolve in
            switch resolution {
            case .Rejected:
                resolve(resolution)
            case .Fulfilled(let value):
                contain_zalgo(q) {
                    resolve(.Fulfilled(body(value as! T)))
                }
            }
        }
    }

    public func then<U>(on q: dispatch_queue_t = dispatch_get_main_queue(), _ body: (T) -> Promise<U>) -> Promise<U> {
        return Promise<U>(when: self) { resolution, resolve in
            switch resolution {
            case .Rejected:
                resolve(resolution)
            case .Fulfilled(let value):
                contain_zalgo(q) {
                    body(value as! T).pipe(resolve)
                }
            }
        }
    }

    public func thenInBackground<U>(body: (T) -> U) -> Promise<U> {
        return then(on: dispatch_get_global_queue(0, 0), body)
    }

    public func thenInBackground<U>(body: (T) -> Promise<U>) -> Promise<U> {
        return then(on: dispatch_get_global_queue(0, 0), body)
    }

    public func error(policy policy: CatchPolicy = .AllErrorsExceptCancellation, _ body: (NSError) -> Void) {
        pipe { resolution in
            switch resolution {
            case .Fulfilled:
                break
            case .Rejected(let error):
                dispatch_async(dispatch_get_main_queue()) {
                    if policy == .AllErrors || !error.cancelled {
                        consume(error)
                        body(error)
                    }
                }
            }
        }
    }

    public func recover(on q: dispatch_queue_t = dispatch_get_main_queue(), _ body: (NSError) -> Promise<T>) -> Promise<T> {
        return Promise(when: self) { resolution, resolve in
            switch resolution {
            case .Rejected(let error):
                contain_zalgo(q) {
                    consume(error)
                    body(error).pipe(resolve)
                }
            case .Fulfilled:
                resolve(resolution)
            }
        }
    }

    public func finally(on q: dispatch_queue_t = dispatch_get_main_queue(), _ body: () -> Void) -> Promise<T> {
        return Promise(when: self) { resolution, resolve in
            contain_zalgo(q) {
                body()
                resolve(resolution)
            }
        }
    }

    public var value: T? {
        switch state.get() {
        case .None:
            return nil
        case .Some(.Fulfilled(let value)):
            return (value as! T)
        case .Some(.Rejected):
            return nil
        }
    }
}


public let zalgo: dispatch_queue_t = dispatch_queue_create("Zalgo", nil)

public let waldo: dispatch_queue_t = dispatch_queue_create("Waldo", nil)

func contain_zalgo(q: dispatch_queue_t, block: () -> Void) {
    if q === zalgo {
        block()
    } else if q === waldo {
        if NSThread.isMainThread() {
            dispatch_async(dispatch_get_global_queue(0, 0), block)
        } else {
            block()
        }
    } else {
        dispatch_async(q, block)
    }
}


extension Promise {
    public convenience init(error: String, code: Int = Constants.PMKUnexpectedError) {
        let error = NSError(domain: Constants.PMKErrorDomain, code: code, userInfo: [NSLocalizedDescriptionKey: error])
        self.init(error)
    }

    public func asAny() -> Promise<Any> {
        return Promise<Any>(passthru: pipe)
    }

    public func asAnyObject() -> Promise<AnyObject> {
        return Promise<AnyObject>(passthru: pipe)
    }

    /**
    Swift (1.2) seems to be much less fussy about Void promises.
    */
    public func asVoid() -> Promise<Void> {
        return then(on: zalgo) { _ in return }
    }
}


extension Promise: CustomDebugStringConvertible {
    public var debugDescription: String {
        return "Promise: \(state)"
    }
}

public func firstly<T>(promise: () -> Promise<T>) -> Promise<T> {
    return promise()
}

public enum ErrorPolicy {
    case AllErrors
    case AllErrorsExceptCancellation
}

extension Promise {
    public var error: NSError? {
        switch state.get() {
        case .None:
            return nil
        case .Some(.Fulfilled):
            return nil
        case .Some(.Rejected(let error)):
            return error
        }
    }

    public var pending: Bool {
        return state.get() == nil
    }

    public var resolved: Bool {
        return !pending
    }

    public var fulfilled: Bool {
        return value != nil
    }

    public var rejected: Bool {
        return error != nil
    }
}

public var PMKUnhandledErrorHandler = { (error: NSError) -> Void in
    dispatch_async(dispatch_get_main_queue()) {
        if !error.cancelled {
            NSLog("PromiseKit: Unhandled error: %@", error)
        }
    }
}

private class Consumable: NSObject {
    let parentError: NSError
    var consumed: Bool = false

    deinit {
        if !consumed {
            PMKUnhandledErrorHandler(parentError)
        }
    }

    init(parent: NSError) {
        parentError = parent.copy() as! NSError
    }
}

private var handle: UInt8 = 0

func consume(error: NSError) {
    if let pmke = objc_getAssociatedObject(error, &handle) as? Consumable {
        pmke.consumed = true
    }
}

func unconsume(error: NSError) {
    if let pmke = objc_getAssociatedObject(error, &handle) as! Consumable? {
        pmke.consumed = false
    } else {
        objc_setAssociatedObject(error, &handle, Consumable(parent: error), .OBJC_ASSOCIATION_RETAIN)
    }
}

private struct ErrorPair: Hashable {
    let domain: String
    let code: Int
    init(_ d: String, _ c: Int) {
        domain = d; code = c
    }
    var hashValue: Int {
        return "\(domain):\(code)".hashValue
    }
}

private func ==(lhs: ErrorPair, rhs: ErrorPair) -> Bool {
    return lhs.domain == rhs.domain && lhs.code == rhs.code
}

private var cancelledErrorIdentifiers = Set([
    ErrorPair(Constants.PMKErrorDomain, Constants.PMKOperationCancelled),
    ErrorPair(NSURLErrorDomain, NSURLErrorCancelled)
    ])

extension NSError {
    public class func cancelledError() -> NSError {
        let info: [String: AnyObject] = [NSLocalizedDescriptionKey: "The operation was cancelled"]
        return NSError(domain: Constants.PMKErrorDomain, code: Constants.PMKOperationCancelled, userInfo: info)
    }

    public class func registerCancelledErrorDomain(domain: String, code: Int) {
        cancelledErrorIdentifiers.insert(ErrorPair(domain, code))
    }

    public var cancelled: Bool {
        return cancelledErrorIdentifiers.contains(ErrorPair(domain, code))
    }
}

public class Sealant<T> {
    let handler: (Resolution) -> ()

    init(body: (Resolution) -> Void) {
        handler = body
    }

    func __resolve(obj: AnyObject) {
        switch obj {
        case is NSError:
            resolve(obj as! NSError)
        default:
            handler(.Fulfilled(obj))
        }
    }

    public func resolve(value: T) {
        handler(.Fulfilled(value))
    }

    public func resolve(error: NSError!) {
        unconsume(error)
        handler(.Rejected(error))
    }

    public func resolve(obj: T?, var _ error: NSError?) {
        if let obj = obj {
            handler(.Fulfilled(obj))
        } else if let error = error {
            resolve(error)
        } else {
            //FIXME couldn't get the constants from the umbrella header :(
            error = NSError(domain: Constants.PMKErrorDomain, code: /*PMKUnexpectedError*/ 1, userInfo: nil)
            resolve(error)
        }
    }

    public func resolve(obj: T, _ error: NSError?) {
        if error == nil {
            handler(.Fulfilled(obj))
        } else  {
            resolve(error)
        }
    }
}

enum Resolution {
    case Fulfilled(Any)    //TODO make type T when Swift can handle it
    case Rejected(NSError)
}

enum Seal {
    case Pending(Handlers)
    case Resolved(Resolution)
}

protocol State {
    func get() -> Resolution?
    func get(body: (Seal) -> Void)
}

class UnsealedState: State {
    private let barrier = dispatch_queue_create("org.promisekit.barrier", DISPATCH_QUEUE_CONCURRENT)
    private var seal: Seal

    func get() -> Resolution? {
        var result: Resolution?
        dispatch_sync(barrier) {
            switch self.seal {
            case .Resolved(let resolution):
                result = resolution
            case .Pending:
                break
            }
        }
        return result
    }

    func get(body: @escaping (Seal) -> Void) {
        var sealed = false
        dispatch_sync(barrier) {
            switch self.seal {
            case .Resolved:
                sealed = true
            case .Pending:
                sealed = false
            }
        }
        if !sealed {
            dispatch_barrier_sync(barrier) {
                switch (self.seal) {
                case .Pending:
                    body(self.seal)
                case .Resolved:
                    sealed = true  // welcome to race conditions
                }
            }
        }
        if sealed {
            body(seal)
        }
    }

    init(resolver: inout ((Resolution) -> Void)!) {
        seal = .Pending(Handlers())
        resolver = { resolution in
            var handlers: Handlers?
            dispatch_barrier_sync(self.barrier) {
                switch self.seal {
                case .Pending(let hh):
                    self.seal = .Resolved(resolution)
                    handlers = hh
                case .Resolved:
                    break
                }
            }
            if let handlers = handlers {
                for handler in handlers {
                    handler(resolution)
                }
            }
        }
    }
}

class SealedState: State {
    private let resolution: Resolution

    init(resolution: Resolution) {
        self.resolution = resolution
    }

    func get() -> Resolution? {
        return resolution
    }
    func get(body: (Seal) -> Void) {
        body(.Resolved(resolution))
    }
}


class Handlers: SequenceType {
    var bodies: [(Resolution)->()] = []

    func append(body: (Resolution)->()) {
        bodies.append(body)
    }

    func generate() -> IndexingGenerator<[(Resolution)->()]> {
        return bodies.generate()
    }

    var count: Int {
        return bodies.count
    }
}


extension Resolution: CustomDebugStringConvertible {
    var debugDescription: String {
        switch self {
        case Fulfilled(let value):
            return "Fulfilled with value: \(value)"
        case Rejected(let error):
            return "Rejected with error: \(error)"
        }
    }
}

extension UnsealedState: CustomDebugStringConvertible {
    var debugDescription: String {
        var rv: String?
        get { seal in
            switch seal {
            case .Pending(let handlers):
                rv = "Pending with \(handlers.count) handlers"
            case .Resolved(let resolution):
                rv = "\(resolution)"
            }
        }
        return "UnsealedState: \(rv!)"
    }
}

extension SealedState: CustomDebugStringConvertible {
    var debugDescription: String {
        return "SealedState: \(resolution)"
    }
}


struct Constants {
    static let PMKErrorDomain = "PMKErrorDomain"
    static let PMKUnexpectedError = 1
    static let PMKOperationCancelled = 5
}
