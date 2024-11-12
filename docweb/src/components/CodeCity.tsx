import { Unity, useUnityContext } from "react-unity-webgl";
import { Box } from "@mui/material";

const buildDir = "build"; //directory of webgl build
const buildName = buildDir; //name of build (set in unity when creating build)
const compression = ""; //("", ".gz", ".br") depending on build settings

const CodeCity = () => {

  const { unityProvider } = useUnityContext({
    loaderUrl: `${buildDir}/Build/${buildName}.loader.js`,
    dataUrl: `${buildDir}/Build/${buildName}.data${compression}`,
    frameworkUrl: `${buildDir}/Build/${buildName}.framework.js${compression}`,
    codeUrl: `${buildDir}/Build/${buildName}.wasm${compression}`,
  });

  return (<Box
      display="flex"
      flexDirection="column"
      justifyContent="center"
      alignItems="center"
      height="100%"
      textAlign="center"
    > 
      <Unity unityProvider={unityProvider} style={{width: "90%", height: "90%"}} />
    </Box>);
}

export default CodeCity;
