import { Link } from "react-router-dom";

import {
  List,
  ListItemButton,
  ListItemText,
  Accordion,
  AccordionSummary,
  AccordionDetails,
  Box,
  ListItemIcon,
	Icon,
} from "@mui/material";

import { blue } from "@mui/material/colors";

import CityIcon from "@mui/icons-material/Apartment";
import HomeIcon from "@mui/icons-material/Home";
import ExpandMoreIcon from "@mui/icons-material/ExpandMore";
import InsertDriveFileIcon from "@mui/icons-material/InsertDriveFile";

import FileList from "./FileList";

import { RepoDescription } from "../types/description";

interface Props {
  documentation: RepoDescription;
}

const SideMenuButton = ({title, to_url, icon} : Props): JSX.Element => {
return  (<Box
        sx={{
          boxShadow: 2,
          mb: 2,
          borderRadius: 1,
          border: "1px solid #ccc",
        }}
      >
	<ListItemButton component={Link} to={to_url} sx={{ padding: "12px 16px" }}>
          <ListItemIcon>
            <Icon component={icon} />
          </ListItemIcon>
          <ListItemText primary={title} />
        </ListItemButton>
      </Box>)
}

const SideMenu = ({ documentation }: Props): JSX.Element => {
  return (
    <List sx={{ width: 400, height: "100vh", overflow: "hidden" }}>
    <SideMenuButton title="Home" to_url="/" icon={HomeIcon} />
    <SideMenuButton title="Code City" to_url="/codecity" icon={CityIcon} />
      <Accordion
        sx={{
          border: "1px solid #ccc",
          borderRadius: 1,
          boxShadow: 1,
        }}
      >
        <AccordionSummary
          expandIcon={<ExpandMoreIcon sx={{ color: blue[400] }} />}
        >
          <ListItemIcon>
            <InsertDriveFileIcon />
          </ListItemIcon>
          <ListItemText primary="Files" />
        </AccordionSummary>
        <AccordionDetails
          sx={{
            maxHeight: "calc(100vh - 150px)",
            overflowY: "auto",
          }}
        >
          <FileList files={documentation.files} />
        </AccordionDetails>
      </Accordion>
    </List>
  );
};

export default SideMenu;