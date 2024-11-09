import { useParams } from "react-router-dom";
import { Typography } from "@mui/material";

import FunctionParametersTable from "../FunctionDocDisplay/FunctionParametersTable";
import FunctionReturnsTable from "../FunctionDocDisplay/FunctionReturnTable";

import { ClassDescription } from "../types/description";
import { RepoDescription } from "../types/description";

const style = {
  border: "1px solid #ccc",
  borderRadius: "8px",
  padding: "20px",
  marginBottom: "20px",
};

const ClassDocDisplay = ({
  documentation,
}: {
  documentation: RepoDescription;
}): JSX.Element | null => {
  const { className } = useParams<{ className: string }>();

  const classInfo = className ? findClassInfo(className, documentation) : null;

  if (!classInfo) {
    return null;
  }

  return (
    <div>
      <Typography variant="h4" gutterBottom>
        {classInfo.name}
      </Typography>
      <Typography variant="body1" color="textSecondary" gutterBottom>
        {classInfo.description}
      </Typography>
      {classInfo.methods && classInfo.methods.length > 0 && (
        <>
          <Typography variant="h6" sx={{ mt: 3, mb: 3 }}>
            Class Methods
          </Typography>
          {classInfo.methods.map((method, index) => (
            <div key={index} style={style}>
              <Typography variant="h6" sx={{ mt: 1, mb: 3 }}>
                {method.name}
              </Typography>
              <Typography variant="body1" color="textSecondary">
                {method.description}
              </Typography>
              {method.params && method.params.length > 0 && (
                <>
                  <Typography variant="h6" sx={{ mt: 3 }}>
                    Parameters
                  </Typography>
                  <FunctionParametersTable params={method.params} />
                </>
              )}
              {method.returns && (
                <>
                  <Typography variant="h6" sx={{ mt: 3, mb: 3 }}>
                    Returns
                  </Typography>
                  <FunctionReturnsTable returns={method.returns} />
                </>
              )}
            </div>
          ))}
        </>
      )}
    </div>
  );
};

const findClassInfo = (
  className: string,
  documentation: RepoDescription
): ClassDescription | null => {
  for (const file of documentation.files) {
    const classInfo = file.classes?.find((cls) => cls.name === className);
    if (classInfo) {
      return classInfo;
    }
  }
  return null;
};

export default ClassDocDisplay;
