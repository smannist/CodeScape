import { Link as RouterLink } from "react-router-dom";

import { List, ListItemButton, ListItemText, Typography } from "@mui/material";

import { FunctionDescription } from "../types/description";

interface Props {
  functions: FunctionDescription[];
}

const FunctionList = ({ functions }: Props): JSX.Element => {
  return (
    <List component="div" disablePadding>
      <Typography variant="subtitle1" sx={{ color: "gray", mt: 1 }}>
        Functions
      </Typography>
      {functions.map((func, idx) => (
        <ListItemButton
          key={idx}
          component={RouterLink}
          to={`/function/${func.name}`}
          sx={{ pl: 1 }}
        >
          <ListItemText primary={func.name} />
        </ListItemButton>
      ))}
    </List>
  );
};

export default FunctionList;
