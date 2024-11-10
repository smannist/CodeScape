import {
  List,
  Accordion,
  AccordionSummary,
  AccordionDetails,
  Typography,
  Divider,
} from "@mui/material";

import { blue } from "@mui/material/colors";

import ExpandMoreIcon from "@mui/icons-material/ExpandMore";

import ClassList from "./ClassList";
import FunctionList from "./FunctionList";

import { FileDescription } from "../types/description";

interface Props {
  files: FileDescription[];
}

const FileList = ({ files }: Props): JSX.Element => {
  const filteredFiles = files
    .map((file) => ({
      ...file,
      name:
        file.name
          .split("/")
          .pop()
          ?.replace(/\.[^/.]+$/, "") || file.name,
    }))
    .filter(
      (file) =>
        (file.classes && file.classes.length > 0) ||
        (file.functions && file.functions.length > 0)
    );

  return (
    <List component="div" disablePadding>
      {filteredFiles.map((file, idx) => (
        <Accordion
          key={idx}
          sx={{
            mb: 2,
            border: "1px solid #ccc",
            borderRadius: 1,
            boxShadow: 1,
          }}
        >
          <AccordionSummary
            expandIcon={<ExpandMoreIcon sx={{ color: blue[400] }} />}
          >
            <Typography variant="subtitle1" sx={{ fontWeight: "bold" }}>
              {file.name}
            </Typography>
          </AccordionSummary>
          <AccordionDetails>
            <Divider sx={{ my: 1 }} />
            {file.classes && file.classes.length > 0 ? (
              <ClassList classes={file.classes} />
            ) : null}
            {file.functions && file.functions.length > 0 ? (
              <FunctionList functions={file.functions} />
            ) : null}
          </AccordionDetails>
        </Accordion>
      ))}
    </List>
  );
};

export default FileList;
