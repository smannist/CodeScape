import { useParams } from "react-router-dom";

import { Typography } from "@mui/material";

import FunctionParametersTable from "./FunctionParametersTable";
import FunctionReturnTable from "./FunctionReturnTable";

import { FunctionDescription } from "../types/description";
import { RepoDescription } from "../types/description";

const style = {
  border: "1px solid #ccc",
  borderRadius: "8px",
  padding: "20px",
  marginBottom: "20px",
};

const FunctionDocDisplay = ({
  documentation,
}: {
  documentation: RepoDescription;
}): JSX.Element | null => {
  const { functionName } = useParams<{ functionName: string | undefined }>();

  const functionInfo = functionName
    ? findFunctionInfo(functionName, documentation)
    : null;

  if (!functionInfo) {
    return null;
  }

  return (
    <div>
      <Typography variant="h4" gutterBottom>
        {functionInfo.name}
      </Typography>
      <Typography
        variant="body1"
        color="textSecondary"
        gutterBottom
        sx={{ mb: 2 }}
      >
        {functionInfo.description}
      </Typography>
      {(functionInfo.params || functionInfo.returns) && (
        <div style={style}>
          {functionInfo.params && functionInfo.params.length > 0 && (
            <>
              <Typography variant="h6" sx={{ mt: 1 }}>
                Parameters
              </Typography>
              <FunctionParametersTable params={functionInfo.params} />
            </>
          )}

          {functionInfo.returns && (
            <>
              <Typography variant="h6" sx={{ mt: 1 }}>
                Returns
              </Typography>
              <FunctionReturnTable returns={functionInfo.returns} />
            </>
          )}
        </div>
      )}
    </div>
  );
};

const findFunctionInfo = (
  functionName: string,
  documentation: RepoDescription
): FunctionDescription | null => {
  for (const file of documentation.files) {
    const funcInfo = file.functions?.find((func) => func.name === functionName);
    if (funcInfo) {
      return funcInfo;
    }
  }
  return null;
};

export default FunctionDocDisplay;
