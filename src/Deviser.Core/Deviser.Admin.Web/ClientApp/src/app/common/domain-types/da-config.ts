export interface DAConfig {
    isEmbedded: boolean, //true for embedded in admin module, false for standalone
    debugBaseUrl: string,
    module: string,
    model: string,
    assetPath: string
}